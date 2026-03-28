using System.Text.Json;
using Azure.Storage.Blobs;
using Business.Mappings;
using Business.UseCases;
using Data.Context;
using Data.Entities;
using Data.Enums;
using Data.Repositories.Implementations;
using Data.Repositories.Interfaces;
using Data.Services.Implementations;
using Data.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Database & Azure
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton(new BlobServiceClient(
    builder.Configuration.GetConnectionString("AzuriteBlob")));


// Identity Configuration
// --------------------------------------
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = true;

    options.User.RequireUniqueEmail = true;

    options.Lockout.AllowedForNewUsers = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();


// JWT Authentication
// --------------------------------------
var jwtSecret = builder.Configuration["Jwt:Secret"]!;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";

            var isExpired = context.AuthenticateFailure is Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException;
            var message = isExpired
                ? "Tu sesión ha expirado por inactividad tras 15 minutos"
                : "Acceso denegado: no tienes permisos";

            var json = System.Text.Json.JsonSerializer.Serialize(new { errors = new[] { message } });
            return context.Response.WriteAsync(json);
        },
        OnForbidden = context =>
        {
            context.Response.StatusCode = 403;
            context.Response.ContentType = "application/json";
            var json = System.Text.Json.JsonSerializer.Serialize(new
            {
                errors = new[] { "Acceso denegado: no tienes permisos" }
            });
            return context.Response.WriteAsync(json);
        }
    };
});


// Services
// --------------------------------------
builder.Services.AddScoped<IMediaStorageService, AzureMediaStorageService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();


// Repositories (Merged)
// --------------------------------------
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<ICursoRepository, CursoRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ICatalogoRepository, CatalogoRepository>();
builder.Services.AddScoped<IDocenteRepository, DocenteRepository>();

builder.Services.AddScoped<ILessonProgressRepository, LessonProgressRepository>();
builder.Services.AddScoped<IInscriptionRepository, InscriptionRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IEvaluationRepository, EvaluationRepository>();


// UseCases (Merged)
// --------------------------------------

// Auth & Users
builder.Services.AddScoped<LoginUseCase>();
builder.Services.AddScoped<RefreshTokenUseCase>();
builder.Services.AddScoped<CreateUserUseCase>();
builder.Services.AddScoped<ListUsersUseCase>();

// Storage & Common
builder.Services.AddScoped<UploadImageUseCase>();
builder.Services.AddScoped<CreatePersonUseCase>();
builder.Services.AddScoped<ListPersonsUseCase>();

// Categorias
builder.Services.AddScoped<Business.UseCases.Categorias.CreateCategoriaUseCase>();
builder.Services.AddScoped<Business.UseCases.Categorias.ListCategoriasUseCase>();
builder.Services.AddScoped<Business.UseCases.Categorias.GetCategoriaByIdUseCase>();
builder.Services.AddScoped<Business.UseCases.Categorias.UpdateCategoriaUseCase>();
builder.Services.AddScoped<Business.UseCases.Categorias.DeleteCategoriaUseCase>();

// Catalogos
builder.Services.AddScoped<Business.UseCases.Catalogos.CreateCatalogoUseCase>();
builder.Services.AddScoped<Business.UseCases.Catalogos.ListCatalogosUseCase>();
builder.Services.AddScoped<Business.UseCases.Catalogos.GetCatalogoByIdUseCase>();
builder.Services.AddScoped<UpdatePersonUseCase>();
builder.Services.AddScoped<ChangePersonStatusUseCase>();

//Inscriptions
builder.Services.AddScoped<CreateInscriptionUseCase>();
builder.Services.AddScoped<ListInscriptionsUseCase>();
builder.Services.AddScoped<CancelInscriptionUseCase>();
// Evaluations
builder.Services.AddScoped<CreateEvaluationUseCase>();
builder.Services.AddScoped<AddEvaluationQuestionUseCase>();
builder.Services.AddScoped<SubmitEvaluationUseCase>();
builder.Services.AddScoped<ListMyEvaluationGradesUseCase>();
builder.Services.AddScoped<ListEvaluationGradesForTeacherUseCase>();
builder.Services.AddScoped<GetEvaluationToTakeUseCase>();
// Users
builder.Services.AddScoped<CreateUserUseCase>();
builder.Services.AddScoped<ListUsersUseCase>();
builder.Services.AddScoped<UpdateUserUseCase>();
builder.Services.AddScoped<ChangeUserStatusUseCase>();
builder.Services.AddScoped<ResetUserPasswordUseCase>();

// Auth
builder.Services.AddScoped<LoginUseCase>();
builder.Services.AddScoped<RefreshTokenUseCase>();
builder.Services.AddScoped<LogoutUseCase>();
builder.Services.AddScoped<ChangePasswordUseCase>();

// Profile
builder.Services.AddScoped<GetMyProfileUseCase>();
builder.Services.AddScoped<UpdateMyProfileUseCase>();

// Cursos
builder.Services.AddScoped<Business.UseCases.Cursos.CreateCursoUseCase>();
builder.Services.AddScoped<Business.UseCases.Cursos.ListCursosUseCase>();
builder.Services.AddScoped<Business.UseCases.Cursos.GetCursoByIdUseCase>();
builder.Services.AddScoped<Business.UseCases.Cursos.UpdateCursoUseCase>();
builder.Services.AddScoped<Business.UseCases.Cursos.DeleteCursoUseCase>();

// Docentes
builder.Services.AddScoped<Business.UseCases.Docentes.CreateDocenteUseCase>();
builder.Services.AddScoped<Business.UseCases.Docentes.ListDocentesUseCase>();


// Validators & Mappings
// --------------------------------------

builder.Services.AddValidatorsFromAssemblyContaining<PersonProfile>();

builder.Services.AddAutoMapper(
    cfg => { },
    typeof(PersonProfile),
    typeof(UserProfile),
    typeof(InscriptionProfile),
    typeof(CourseProfile),
    typeof(EvaluationProfile));



// HTTP Pipeline & Middleware
// --------------------------------------
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.WithOrigins("http://localhost:5173")
          .AllowAnyHeader()
          .AllowAnyMethod()));

builder.Services.AddControllers();


builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

var app = builder.Build();

// Seed de roles y admin inicial
await Api.Extensions.DbSeeder.SeedRolesAndAdminAsync(app.Services);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

// Helpers
static async Task SeedRolesAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    foreach (var role in UserRoles.All)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}



// Deleted Local Seed Method
internal sealed class BearerSecuritySchemeTransformer(
    Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider authenticationSchemeProvider)
    : Microsoft.AspNetCore.OpenApi.IOpenApiDocumentTransformer
{
    public async Task TransformAsync(
        Microsoft.OpenApi.OpenApiDocument document,
        Microsoft.AspNetCore.OpenApi.OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        var authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();
        if (authenticationSchemes.Any(authScheme => authScheme.Name == "Bearer"))
        {
            document.Components ??= new Microsoft.OpenApi.OpenApiComponents();
            document.Components.SecuritySchemes = new Dictionary<string, Microsoft.OpenApi.IOpenApiSecurityScheme>
            {
                ["Bearer"] = new Microsoft.OpenApi.OpenApiSecurityScheme
                {
                    Type = Microsoft.OpenApi.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    In = Microsoft.OpenApi.ParameterLocation.Header,
                    BearerFormat = "Json Web Token"
                }
            };
        }
    }
}