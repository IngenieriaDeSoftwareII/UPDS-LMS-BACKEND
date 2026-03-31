using Business.DTOs.Responses;
using Data.Context;
using Data.Enums;
using Data.Services.Interfaces;

namespace Business.UseCases.ImageContent;

public class UploadImageContentUseCase
{
    private readonly AppDbContext db;
    private readonly IMediaStorageService storage;
    private const string Container = "images";

    public UploadImageContentUseCase(AppDbContext db, IMediaStorageService storage)
    {
        this.db = db;
        this.storage = storage;
    }

    public async Task<ImageContentDto> ExecuteAsync(
        int lessonId,
        string title,
        Stream fileStream,
        string fileName,
        int order
    )
    {
        // 🔥 VALIDAR EXTENSIÓN
        var ext = Path.GetExtension(fileName).ToLower();

        if (!new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" }.Contains(ext))
            throw new InvalidOperationException($"Tipo de archivo no permitido: {ext}");

        // 🔥 MAPEO CORRECTO (FIX REAL)
        FormatImage formatEnum;

        switch (ext)
        {
            case ".jpg":
            case ".jpeg":
                formatEnum = FormatImage.jpg;
                break;
            case ".png":
                formatEnum = FormatImage.png;
                break;
            case ".gif":
                formatEnum = FormatImage.gif;
                break;
            case ".webp":
                formatEnum = FormatImage.webp;
                break;
            default:
                throw new InvalidOperationException($"Formato no soportado: {ext}");
        }

        // 1. SUBIR ARCHIVO
        var blobName = await storage.UploadAsync(fileStream, fileName, Container);
        var url = await storage.GetReadUrlAsync(blobName, Container, TimeSpan.FromHours(1));

        // 2. CREAR CONTENT
        var content = new Data.Entities.Content
        {
            LeccionId = lessonId,
            Tipo = TypeContent.imagen,
            Titulo = string.IsNullOrWhiteSpace(title) ? fileName : title,
            Orden = order,
            EntityStatus = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        db.Contents.Add(content);
        await db.SaveChangesAsync();

        // 3. CREAR IMAGECONTENT
        var imageContent = new Data.Entities.ImageContent
        {
            ContenidoId = content.Id,
            UrlImagen = blobName,
            TextoAlternativo = title ?? fileName,
            Formato = formatEnum, // ✅ FIX AQUÍ
            TamanoKb = (int)(fileStream.Length / 1024)
        };

        db.ImageContents.Add(imageContent);
        await db.SaveChangesAsync();

        // 🔥 RESPUESTA
        return new ImageContentDto
        {
            ContentId = content.Id,
            ImageUrl = url.ToString(),
            Format = formatEnum.ToString(),
            AltText = imageContent.TextoAlternativo,
            SizeKb = imageContent.TamanoKb,

            Content = new ContentDto
            {
                Id = content.Id,
                LessonId = content.LeccionId,
                Title = content.Titulo,
                Order = content.Orden
            }
        };
    }
}