using Azure.Storage.Blobs;
using Business.Mappings;
using Business.UseCases;
using Business.UseCases.Content;
using Business.UseCases.DocumentContent;
using Business.UseCases.ImageContent;
using Business.UseCases.Lesson;
using Business.UseCases.VideoContent;
using Data.Context;
using Data.Repositories.Implementations;
using Data.Repositories.Interfaces;
using Data.Services.Implementations;
using Data.Services.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

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

builder.Services.AddScoped<ILessonRepository, LessonRepository>();
builder.Services.AddScoped<IContentRepository, ContentRepository>();
builder.Services.AddScoped<IVideoContentRepository, VideoContentRepository>();
builder.Services.AddScoped<IImageContentRepository, ImageContentRepository>();
builder.Services.AddScoped<IDocumentContentRepository, DocumentContentRepository>();

// UseCases
// --------------------------------------
// Storage
builder.Services.AddScoped<UploadImageUseCase>();

// Persons
builder.Services.AddScoped<CreatePersonUseCase>();
builder.Services.AddScoped<ListPersonsUseCase>();

//Lessons
builder.Services.AddScoped<CreateLessonUseCase>();
builder.Services.AddScoped<ListLessonsUseCase>();
builder.Services.AddScoped<UpdateLessonUseCase>();
builder.Services.AddScoped<DeleteLessonUseCase>();

//Content
builder.Services.AddScoped<CreateContentUseCase>();
builder.Services.AddScoped<ListContentsUseCase>();
builder.Services.AddScoped<UpdateContentUseCase>();
builder.Services.AddScoped<DeleteContentUseCase>();

//Video Content
builder.Services.AddScoped<CreateVideoContentUseCase>();
builder.Services.AddScoped<ListVideoContentsUseCase>();
builder.Services.AddScoped<UpdateVideoContentUseCase>();
builder.Services.AddScoped<DeleteVideoContentUseCase>();

//Image Content
builder.Services.AddScoped<CreateImageContentUseCase>();
builder.Services.AddScoped<ListImageContentsUseCase>();
builder.Services.AddScoped<UpdateImageContentUseCase>();
builder.Services.AddScoped<DeleteImageContentUseCase>();

//Document Content
builder.Services.AddScoped<CreateDocumentContentUseCase>();
builder.Services.AddScoped<ListDocumentContentsUseCase>();
builder.Services.AddScoped<UpdateDocumentContentUseCase>();
builder.Services.AddScoped<DeleteDocumentContentUseCase>();



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
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
