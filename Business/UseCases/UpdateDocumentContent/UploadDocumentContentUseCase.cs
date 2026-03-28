using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Data.Context;
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
        // Validar extensión
        var ext = Path.GetExtension(dto.FileName).ToLower();
        if (!new[] { ".pdf", ".docx", ".doc", ".xlsx", ".xls", ".pptx", ".ppt", ".zip", ".rar" }.Contains(ext))
            throw new InvalidOperationException($"Tipo de archivo no permitido: {ext}");

        // Subir archivo → devuelve blobName
        var blobName = await _storage.UploadAsync(dto.FileStream, dto.FileName, Container);

        // Crear Content
        var content = new Data.Entities.Content
        {
            LeccionId = dto.LessonId,
            Tipo = TypeContent.documento,
            Titulo = dto.Title,
            Orden = dto.Order,
            EntityStatus = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _db.Contents.Add(content);
        await _db.SaveChangesAsync();

        // Guardar SOLO blobName
        var documentContent = new Data.Entities.DocumentContent
        {
            ContenidoId = content.Id,
            UrlArchivo = blobName,
            Formato = Enum.Parse<FormatDocument>(dto.Format, true),
            TamanoKb = dto.SizeKb,
            NumPaginas = dto.PageCount
        };

        _db.DocumentContents.Add(documentContent);
        await _db.SaveChangesAsync();

        // Respuesta
        return new UploadDocumentContentResponseDto
        {
            ContentId = content.Id,
            DocumentId = documentContent.ContenidoId,
            FileName = dto.FileName,
            FileUrl = blobName,
            LessonId = dto.LessonId,
            CreatedAt = content.CreatedAt ?? DateTime.UtcNow
        };
    }
}