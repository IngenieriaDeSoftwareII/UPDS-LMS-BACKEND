using Business.DTOs.Responses;
using Data.Repositories.Interfaces;
using Data.Services.Interfaces;

namespace Business.UseCases.VideoContent;

public class ListVideoContentsUseCase
{
    private readonly IVideoContentRepository _repository;
    private readonly IMediaStorageService _storage;
    private const string Container = "videos";

    public ListVideoContentsUseCase(
        IVideoContentRepository repository,
        IMediaStorageService storage)
    {
        _repository = repository;
        _storage = storage;
    }

    public async Task<IEnumerable<VideoContentDto>> ExecuteAsync()
    {
        var videos = await _repository.GetAllWithContentAsync();

        var result = new List<VideoContentDto>();

        foreach (var v in videos)
        {
            string url = "";

            if (!string.IsNullOrWhiteSpace(v.UrlVideo))
            {
                try
                {
                    var uri = await _storage.GetReadUrlAsync(
                        v.UrlVideo,
                        Container,
                        TimeSpan.FromHours(1)
                    );

                    url = uri.ToString();
                }
                catch
                {
                    url = ""; // fallback
                }
            }

            result.Add(new VideoContentDto
            {
                ContentId = v.ContenidoId,
                UrlVideo = url,
                DuracionSeg = v.DuracionSeg,

                Content = v.Contenido == null ? null : new ContentDto
                {
                    Id = v.Contenido.Id,
                    LessonId = v.Contenido.LeccionId,
                    Title = v.Contenido.Titulo,
                    Order = v.Contenido.Orden
                }
            });
        }

        return result;
    }
}