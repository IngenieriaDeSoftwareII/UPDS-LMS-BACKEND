using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Repositories.Interfaces;
using Data.Services.Interfaces;

namespace Business.UseCases.VideoContent;

public class UpdateVideoContentUseCase(
    IVideoContentRepository repository,
    IMediaStorageService storage)
{
    private const string Container = "videos";

    public async Task<Result<VideoContentDto>> ExecuteAsync(
        int contentId,
        UpdateVideoContentDto dto,
        Stream? fileStream,
        string? fileName)
    {
        var existing = await repository.GetByContentIdAsync(contentId);

        if (existing is null)
            return Result<VideoContentDto>.Failure(["Video no encontrado"]);

        // actualizar duración
        if (dto.DuracionSeg.HasValue)
            existing.DuracionSeg = dto.DuracionSeg.Value;

        // actualizar orden
        if (dto.Order.HasValue)
            existing.Contenido.Orden = dto.Order.Value;

        // actualizar lección
        if (dto.LessonId.HasValue)
            existing.Contenido.LeccionId = dto.LessonId.Value;

        // reemplazar video
        if (fileStream != null && fileName != null)
        {
            var blobName = await storage.UploadAsync(fileStream, fileName, Container);
            existing.UrlVideo = blobName;
        }

        await repository.UpdateAsync(existing);

        var url = await storage.GetReadUrlAsync(existing.UrlVideo, Container, TimeSpan.FromHours(1));

        return Result<VideoContentDto>.Success(new VideoContentDto
        {
            ContentId = existing.ContenidoId,
            UrlVideo = url.ToString(),
            DuracionSeg = existing.DuracionSeg,
            Content = new ContentDto
            {
                Id = existing.Contenido.Id,
                LessonId = existing.Contenido.LeccionId,
                Title = existing.Contenido.Titulo,
                Order = existing.Contenido.Orden
            }
        });
    }
}