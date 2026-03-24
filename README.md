# UPDS LMS — Backend

Backend del Sistema de Gestión de Aprendizaje de la UPDS, desarrollado en .NET 10 con una arquitectura de tres capas inspirada en Clean Architecture.

---

## Requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

---

## Levantar el entorno local

El proyecto depende de dos servicios que corren en Docker: SQL Server y Azurite (emulador de Azure Storage).

```bash
docker-compose up -d
```

| Contenedor           | Imagen                         | Puerto                  |
| -------------------- | ------------------------------ | ----------------------- |
| `upds-lms-sqlserver` | SQL Server 2022                | `1433`                  |
| `upds-lms-azurite`   | Azurite (Blob / Queue / Table) | `10000 / 10001 / 10002` |

SQL Server tiene un healthcheck configurado — espera a que esté listo antes de conectar la API.

---

## Arquitectura

El proyecto está dividido en tres capas con dependencias unidireccionales:

```
Api  ──►  Business  ──►  Data
```

- `Api` solo conoce `Business`.
- `Business` solo conoce `Data`.
- `Data` no conoce a ninguna otra capa.

### Estructura de carpetas

```
UPDS-LMS-BACKEND/
│
├── Api/
│   ├── Controllers/          # Controladores REST, uno por entidad o dominio
│   ├── Middlewares/          # Middlewares personalizados
│   ├── Requests/             # Archivos .http para probar endpoints
│   └── Program.cs            # Configuración de la app y registro de dependencias
│
├── Business/
│   ├── DTOs/
│   │   ├── Requests/         # DTOs de entrada (lo que el cliente envía)
│   │   └── Responses/        # DTOs de respuesta (lo que la API devuelve)
│   ├── Mappings/             # Perfiles de AutoMapper, uno por entidad
│   ├── Results/              # Tipo genérico Result<T> para manejo de resultados
│   ├── UseCases/             # Un archivo por operación (CreateX, ListX, UpdateX...)
│   └── Validators/           # Validadores de FluentValidation, uno por DTO de entrada
│
└── Data/
    ├── Context/              # AppDbContext
    ├── Entities/             # Entidades de dominio (tablas)
    ├── Enums/                # Enumeraciones del dominio
    ├── Migrations/           # Migraciones generadas por EF Core (no editar a mano)
    ├── Repositories/
    │   ├── Interfaces/       # Contratos de repositorios
    │   └── Implementations/  # Implementaciones con EF Core
    └── Services/
        ├── Interfaces/       # Contratos de servicios externos (storage, email, etc.)
        └── Implementations/  # Implementaciones concretas
```

---

## Agregar un nuevo feature

El flujo siempre sigue el mismo orden, de adentro hacia afuera: `Data` → `Business` → `Api`.

### 1. Entidad — `Data/Entities/`

```csharp
public class Course
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}
```

Si necesitas un enum, agrégalo en `Data/Enums/`.

### 2. Registrar en el contexto — `Data/Context/AppDbContext.cs`

```csharp
public DbSet<Course> Courses { get; set; }
```

### 3. Repositorio — `Data/Repositories/`

Interfaz en `Interfaces/`, implementación en `Implementations/`.

```csharp
// Interfaces/ICourseRepository.cs
public interface ICourseRepository
{
    Task<Course> CreateAsync(Course course);
    Task<IEnumerable<Course>> GetAllAsync();
}
```

### 4. DTOs — `Business/DTOs/`

Los DTOs de entrada van en `Requests/`, los de respuesta en `Responses/`. El nombre del archivo incluye el verbo para los de entrada.

```csharp
// DTOs/Requests/CreateCourseDto.cs
public class CreateCourseDto
{
    public string Name { get; set; } = null!;
}

// DTOs/Responses/CourseDto.cs
public class CourseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}
```

### 5. Mapper — `Business/Mappings/`

```csharp
// Mappings/CourseProfile.cs
public class CourseProfile : Profile
{
    public CourseProfile()
    {
        CreateMap<CreateCourseDto, Course>();
        CreateMap<Course, CourseDto>();
    }
}
```

### 6. Validador — `Business/Validators/`

```csharp
// Validators/CreateCourseDtoValidator.cs
public class CreateCourseDtoValidator : AbstractValidator<CreateCourseDto>
{
    public CreateCourseDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}
```

### 7. Caso de uso — `Business/UseCases/`

Los casos de uso retornan `Result<T>` para manejar fallos de forma anticipada sin lanzar excepciones.

```csharp
// UseCases/CreateCourseUseCase.cs
public class CreateCourseUseCase(
    ICourseRepository repository,
    IMapper mapper,
    IValidator<CreateCourseDto> validator)
{
    public async Task<Result<CourseDto>> ExecuteAsync(CreateCourseDto dto)
    {
        var validation = await validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<CourseDto>.Failure(validation.Errors.Select(e => e.ErrorMessage));

        var course = mapper.Map<Course>(dto);
        var created = await repository.CreateAsync(course);

        return Result<CourseDto>.Success(mapper.Map<CourseDto>(created));
    }
}
```

### 8. Controlador — `Api/Controllers/`

El controlador interpreta el `Result<T>` y decide el código de respuesta. No contiene lógica de negocio.

```csharp
// Controllers/CoursesController.cs
[ApiController]
[Route("api/[controller]")]
public class CoursesController(CreateCourseUseCase createCourse) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateCourseDto dto)
    {
        var result = await createCourse.ExecuteAsync(dto);

        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors });

        return CreatedAtAction(nameof(Create), new { id = result.Value!.Id }, result.Value);
    }
}
```

### 9. Registrar en Program.cs

```csharp
// Repositories
builder.Services.AddScoped<ICourseRepository, CourseRepository>();

// UseCases
builder.Services.AddScoped<CreateCourseUseCase>();

// Validators
builder.Services.AddValidatorsFromAssemblyContaining<PersonProfile>();

// Mappings
builder.Services.AddAutoMapper(cfg => { }, typeof(PersonProfile));
```

---

## Result\<T\>

`Business/Results/Result<T>` es el tipo de retorno estándar para cualquier caso de uso que pueda fallar. Evita el uso de excepciones para flujos esperados como validaciones y hace explícito en la firma del método que la operación puede no tener éxito.

```csharp
// Retornar éxito
Result<CourseDto>.Success(courseDto);

// Retornar fallo con mensajes de error
Result<CourseDto>.Failure(["El nombre es requerido.", "El nombre no puede superar 200 caracteres."]);
```

El controlador siempre verifica `result.IsSuccess` antes de acceder a `result.Value`.

Úsalo en operaciones de escritura y en lecturas que puedan no encontrar el recurso. Para lecturas simples que siempre devuelven una colección, retornar directamente el tipo es suficiente.
