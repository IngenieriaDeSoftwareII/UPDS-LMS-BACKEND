using Business.DTOs.Responses;
using Data.Context;
using Data.Enums;
using Data.Services.Interfaces;

namespace Business.UseCases.VideoContent;

public class UploadVideoContentUseCase
{
    private readonly AppDbContext db;
    private readonly IMediaStorageService storage;
    private const string Container = "videos";

    public UploadVideoContentUseCase(AppDbContext db, IMediaStorageService storage)
    {
        this.db = db;
        this.storage = storage;
    }

    public async Task<VideoContentDto> ExecuteAsync(
        int lessonId,
        string title,
        Stream fileStream,
        string fileName,
        int order,
        int duracionSeg
    )
    {
        // 🔥 VALIDAR EXTENSIÓN
        var ext = Path.GetExtension(fileName).ToLower();

        if (!new[] { ".mp4", ".webm", ".ogg" }.Contains(ext))
            throw new InvalidOperationException($"Formato no permitido: {ext}");

        // 1. SUBIR VIDEO
        var blobName = await storage.UploadAsync(fileStream, fileName, Container);
        var url = await storage.GetReadUrlAsync(blobName, Container, TimeSpan.FromHours(1));

        // 2. CREAR CONTENT
        var content = new Data.Entities.Content
        {
            LeccionId = lessonId,
            Tipo = TypeContent.video,
            Titulo = string.IsNullOrWhiteSpace(title) ? fileName : title,
            Orden = order,
            EntityStatus = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        db.Contents.Add(content);
        await db.SaveChangesAsync();

        // 3. CREAR VIDEO CONTENT
        var video = new Data.Entities.VideoContent
        {
            ContenidoId = content.Id,
            UrlVideo = blobName,
            DuracionSeg = duracionSeg
        };

        db.VideoContents.Add(video);
        await db.SaveChangesAsync();

        return new VideoContentDto
        {
            ContentId = content.Id,
            UrlVideo = url.ToString(),
            DuracionSeg = video.DuracionSeg,
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