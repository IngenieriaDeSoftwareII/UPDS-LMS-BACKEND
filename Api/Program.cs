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
builder.Services.AddAutoMapper(cfg => { }, typeof(PersonProfile), typeof(UserProfile));


// HTTP Pipeline & Middleware
// --------------------------------------
builder.Services.AddCors(options => options.AddPolicy("AllowFrontend", policy =>
    policy.WithOrigins("http://localhost:5173")
          .AllowAnyHeader()
          .AllowAnyMethod()));

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Seed de roles al iniciar la aplicación
await SeedRolesAsync(app.Services);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// COMENTADO PARA EVITAR ERROR 307 (Temporary Redirect) Y PROBLEMAS DE CORS EN DESARROLLO LOCAL
// app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

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