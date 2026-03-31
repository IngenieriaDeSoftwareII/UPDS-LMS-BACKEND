using Business.DTOs.Responses;
using Data.Context;
using Data.Entities;
using Data.Enums;

namespace Business.UseCases.VideoContent;

public class CreateVideoWithContentUseCase(AppDbContext db)
{
    public async Task<VideoContentDto> ExecuteAsync(int lessonId, string title, string videoUrl, int durationSeconds)
    {
        // crear contenido base
        var content = new Data.Entities.Content
        {
            LeccionId = lessonId,
            Tipo = TypeContent.video,
            Titulo = title,
            Orden = 1,
            EntityStatus = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        db.Contents.Add(content);
        await db.SaveChangesAsync();

        // crear contenido de video
        var videoContent = new Data.Entities.VideoContent
        {
            ContenidoId = content.Id,
            UrlVideo = videoUrl,
            DuracionSeg = durationSeconds
        };

        db.VideoContents.Add(videoContent);
        await db.SaveChangesAsync();

        return new VideoContentDto
        {
            ContentId = content.Id,
            VideoUrl = videoUrl,
            DurationSeconds = durationSeconds
        };
    }
}
