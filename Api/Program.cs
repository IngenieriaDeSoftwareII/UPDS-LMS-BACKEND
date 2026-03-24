using Azure.Storage.Blobs;
using Business.Mappings;
using Business.UseCases;
using Data.Context;
using Data.Repositories.Implementations;
using Data.Repositories.Interfaces;
using Data.Services.Implementations;
using Data.Services.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton(new BlobServiceClient(
    builder.Configuration.GetConnectionString("AzuriteBlob")));


// Services
// --------------------------------------

builder.Services.AddScoped<IMediaStorageService, AzureMediaStorageService>();


// Repositories
// --------------------------------------

builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ICursoRepository, CursoRepository>();
builder.Services.AddScoped<IDocenteRepository, DocenteRepository>();


// UseCases
// --------------------------------------

// Storage
builder.Services.AddScoped<UploadImageUseCase>();

// Persons
builder.Services.AddScoped<CreatePersonUseCase>();
builder.Services.AddScoped<ListPersonsUseCase>();

// Categorias
builder.Services.AddScoped<Business.UseCases.Categorias.CreateCategoriaUseCase>();
builder.Services.AddScoped<Business.UseCases.Categorias.ListCategoriasUseCase>();
builder.Services.AddScoped<Business.UseCases.Categorias.GetCategoriaByIdUseCase>();
builder.Services.AddScoped<Business.UseCases.Categorias.UpdateCategoriaUseCase>();
builder.Services.AddScoped<Business.UseCases.Categorias.DeleteCategoriaUseCase>();

// Cursos
builder.Services.AddScoped<Business.UseCases.Cursos.CreateCursoUseCase>();
builder.Services.AddScoped<Business.UseCases.Cursos.ListCursosUseCase>();
builder.Services.AddScoped<Business.UseCases.Cursos.GetCursoByIdUseCase>();
builder.Services.AddScoped<Business.UseCases.Cursos.UpdateCursoUseCase>();
builder.Services.AddScoped<Business.UseCases.Cursos.DeleteCursoUseCase>();

// Docentes
builder.Services.AddScoped<Business.UseCases.Docentes.CreateDocenteUseCase>();
builder.Services.AddScoped<Business.UseCases.Docentes.ListDocentesUseCase>();


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

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
