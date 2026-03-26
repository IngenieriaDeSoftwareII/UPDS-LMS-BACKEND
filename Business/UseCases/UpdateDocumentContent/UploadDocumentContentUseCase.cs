using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Data.Context;
using Data.Entities;
using Data.Enums;
using Data.Services.Interfaces;
namespace Business.UseCases.UpdateDocumentContent;

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

    public async Task<UploadDocumentContentResponseDto> ExecuteAsync(UploadDocumentContentRequestDto dto)
    {
        // Validar extensión permitida
        var ext = Path.GetExtension(dto.FileName).ToLower();
        if (!new[] { ".pdf", ".docx", ".doc", ".xlsx", ".xls", ".pptx", ".ppt", ".zip", ".rar" }.Contains(ext))
            throw new InvalidOperationException($"Tipo de archivo no permitido: {ext}");

        // Subir archivo a Azure Blob Storage
        var blobName = await _storage.UploadAsync(dto.FileStream, dto.FileName, Container);

        // Crear entidad Content base
        var content = new Data.Entities.Content
        {
            LessonId = dto.LessonId,
            Type = TypeContent.documento,
            Title = dto.Title,
            Order = 1,
            EntityStatus = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _db.Contents.Add(content);
        await _db.SaveChangesAsync(); // Guarda y genera Content.Id

        // Crear entidad DocumentContent (1:1 con Content)
        var documentContent = new Data.Entities.DocumentContent
        {
            ContentId = content.Id,
            FileUrl = blobName,
            Format = Enum.Parse<FormatDocument>(dto.Format, true),
            SizeKb = dto.SizeKb,
            PageCount = dto.PageCount
        };

        _db.DocumentContents.Add(documentContent);
        await _db.SaveChangesAsync(); // Guarda DocumentContent

        // Retornar respuesta
        return new UploadDocumentContentResponseDto
        {
            ContentId = content.Id,
            DocumentId = documentContent.ContentId,
            FileName = dto.FileName,
            FileUrl = blobName,
            Title = content.Title,
            LessonId = dto.LessonId,
            CreatedAt = content.CreatedAt
        };
    }
}