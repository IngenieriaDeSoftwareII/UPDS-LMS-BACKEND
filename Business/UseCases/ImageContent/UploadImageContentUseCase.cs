using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Data.Context;
using Data.Entities;
using Data.Enums;
using Data.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Business.UseCases.ImageContent;

public class UploadImageContentUseCase(AppDbContext db, IMediaStorageService storage)
{
    private const string Container = "images";

    public async Task<ImageContentDto> ExecuteAsync(int lessonId, string title, Stream fileStream, string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLower().TrimStart('.');
        if (!Enum.TryParse<FormatImage>(extension, true, out var format))
        {
            // por defecto que use jpg si no se reconoce el formato
            format = FormatImage.jpg;
        }

        // subir a storage
        var blobName = await storage.UploadAsync(fileStream, fileName, Container);
        var url = await storage.GetReadUrlAsync(blobName, Container, TimeSpan.FromHours(1));

        // crear contenido base
        var content = new Data.Entities.Content
        {
            LeccionId = lessonId,
            Tipo = TypeContent.imagen,
            Titulo = string.IsNullOrWhiteSpace(title) ? fileName : title,
            Orden = 1,
            EntityStatus = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        db.Contents.Add(content);
        await db.SaveChangesAsync();

        // crear contenido de imagen
        var imageContent = new Data.Entities.ImageContent
        {
            ContenidoId = content.Id,
            UrlImagen = blobName, // store the blob name/path
            Formato = format,
            TextoAlternativo = title ?? fileName,
            TamanoKb = (int)(fileStream.Length / 1024)
        };

        db.ImageContents.Add(imageContent);
        await db.SaveChangesAsync();

        return new ImageContentDto
        {
            ContentId = content.Id,
            ImageUrl = url.ToString(),
            Format = format.ToString(),
            AltText = imageContent.TextoAlternativo,
            SizeKb = imageContent.TamanoKb
        };
    }
}
