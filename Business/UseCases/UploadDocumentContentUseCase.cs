using Business.DTOs.Requests;
using Data.Context;
using Data.Entities;
using Data.Enums;
using Data.Services.Interfaces;

namespace Business.UseCases;

public class UploadDocumentContentUseCase
{
    private readonly AppDbContext _db;
    private readonly IMediaStorageService _storage;
    private const string Container = "documents";

    public UploadDocumentContentUseCase(AppDbContext db, IMediaStorageService storage)
    {
        _db = db;
        _storage = storage;
    }

    public async Task<object> ExecuteAsync(Stream fileStream, string fileName, int lessonId, string title, string format, int? sizeKb = null, int? pageCount = null)
    {
        // Validar extensión permitida
        var ext = Path.GetExtension(fileName).ToLower();
        if (!new[] { ".pdf", ".docx", ".doc", ".xlsx", ".xls", ".pptx", ".ppt", ".zip", ".rar" }.Contains(ext))
            throw new InvalidOperationException($"Tipo de archivo no permitido: {ext}");

        // Subir archivo a Azure Blob Storage
        var blobName = await _storage.UploadAsync(fileStream, fileName, Container);

        // Crear entidad Content base
        var content = new Content
        {
            LessonId = lessonId,
            Type = TypeContent.documento,
            Title = title,
            Order = 1,
            EntityStatus = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _db.Contents.Add(content);
        await _db.SaveChangesAsync(); // Guarda y genera Content.Id

        // Crear entidad DocumentContent (1:1 con Content)
        var documentContent = new DocumentContent
        {
            ContentId = content.Id,
            FileUrl = blobName, // o construir URL completa si lo necesitas
            Format = Enum.Parse<FormatDocument>(format, true),
            SizeKb = sizeKb,
            PageCount = pageCount
        };

        _db.DocumentContents.Add(documentContent);
        await _db.SaveChangesAsync(); // Guarda DocumentContent

        // 5️⃣ Retornar respuesta
        return new
        {
            contentId = content.Id,
            documentId = documentContent.ContentId,
            fileName = fileName,
            fileUrl = blobName,
            title = content.Title,
            lessonId = lessonId,
            createdAt = content.CreatedAt
        };
    }
}