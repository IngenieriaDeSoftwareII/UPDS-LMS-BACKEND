using Business.DTOs.Requests;
using Data.Context;
using Data.Enums;
using Data.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Business.UseCases.UpdateDocumentContent;

public class UpdateDocumentContentUseCase
{
    private readonly AppDbContext _db;
    private readonly IMediaStorageService _storage;
    private const string Container = "documents";

    public UpdateDocumentContentUseCase(AppDbContext db, IMediaStorageService storage)
    {
        _db = db;
        _storage = storage;
    }

    public async Task<bool> ExecuteAsync(int contentId, UpdateDocumentContentDto dto)
    {
        // Obtener documento y su content existente
        var document = await _db.DocumentContents
            .Include(d => d.Content)
            .FirstOrDefaultAsync(d => d.ContentId == contentId);

        if (document == null)
            return false;

        // Actualizar metadata
        document.Content.Title = dto.Title;
        document.Content.Order = dto.Order;
        document.Content.UpdatedAt = DateTime.UtcNow;
        document.PageCount = dto.PageCount;

        // Reemplazar archivo si hay uno nuevo
        if (dto.File != null && dto.File.Length > 0)
        {
            var ext = Path.GetExtension(dto.File.FileName).ToLower();
            if (!new[] { ".pdf", ".docx", ".doc", ".xlsx", ".xls", ".pptx", ".ppt", ".zip", ".rar" }.Contains(ext))
                throw new InvalidOperationException($"Tipo de archivo no permitido: {ext}");

            // Subir archivo y obtener blobName
            var blobName = await _storage.UploadAsync(dto.File.OpenReadStream(), dto.File.FileName, Container);

            document.FileUrl = blobName;
            document.Format = Enum.Parse<FormatDocument>(ext.TrimStart('.'), true);
            document.SizeKb = (int)(dto.File.Length / 1024);
        }

        await _db.SaveChangesAsync();
        return true;
    }
}