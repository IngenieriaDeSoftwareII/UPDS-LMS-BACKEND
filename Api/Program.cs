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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton(new BlobServiceClient(
    builder.Configuration.GetConnectionString("AzuriteBlob")));


// Identity
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


// Services
// --------------------------------------

builder.Services.AddScoped<IMediaStorageService, AzureMediaStorageService>();


// Repositories
// --------------------------------------

builder.Services.AddScoped<IPersonRepository, PersonRepository>();


// UseCases
// --------------------------------------

// Storage
builder.Services.AddScoped<UploadImageUseCase>();

// Persons
builder.Services.AddScoped<CreatePersonUseCase>();
builder.Services.AddScoped<ListPersonsUseCase>();

// Users
builder.Services.AddScoped<CreateUserUseCase>();


// Validators
// --------------------------------------
builder.Services.AddValidatorsFromAssemblyContaining<PersonProfile>();


// Mappings
// --------------------------------------
builder.Services.AddAutoMapper(cfg => { }, typeof(PersonProfile));


builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
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

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();



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