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
                AltText = img.TextoAlternativo,
                SizeKb = img.TamanoKb
            });
        }

        return result;
    }
}