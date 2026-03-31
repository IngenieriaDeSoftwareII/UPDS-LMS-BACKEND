using Business.DTOs.Responses;
using Data.Repositories.Interfaces;
using Data.Services.Interfaces;

namespace Business.UseCases.ImageContent;

public class ListImageContentsUseCase
{
    private readonly IImageContentRepository _repository;
    private readonly IMediaStorageService _storage;

    private const string Container = "images";

    public ListImageContentsUseCase(
        IImageContentRepository repository,
        IMediaStorageService storage
    )
    {
        _repository = repository;
        _storage = storage;
    }

    public async Task<IEnumerable<ImageContentDto>> ExecuteAsync()
    {
        var images = await _repository.GetAllAsync();

        var result = new List<ImageContentDto>();

        foreach (var img in images)
        {
            // Generar URL válida (SAS)
            var url = await _storage.GetReadUrlAsync(
                img.UrlImagen,
                Container,
                TimeSpan.FromHours(1)
            );

            result.Add(new ImageContentDto
            {
                ContentId = img.ContenidoId,
                ImageUrl = url.ToString(),
                Format = img.Formato.ToString(),
                WidthPx = img.AnchoPx,
                HeightPx = img.AltoPx,
                AltText = img.TextoAlternativo,
                SizeKb = img.TamanoKb,
                Content = new ContentDto
                {
                    Id = img.Contenido.Id,
                    LessonId = img.Contenido.LeccionId,
                    Type = img.Contenido.Tipo.ToString(),
                    Title = img.Contenido.Titulo,
                    Order = img.Contenido.Orden,
                    EntityStatus = img.Contenido.EntityStatus,
                    CreatedAt = img.Contenido.CreatedAt ?? DateTime.UtcNow,
                    UpdatedAt = img.Contenido.UpdatedAt ?? DateTime.UtcNow,
                    DeletedAt = img.Contenido.DeletedAt
                }
            });
        }

        return result;
    }
}