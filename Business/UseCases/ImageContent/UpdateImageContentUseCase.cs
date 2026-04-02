using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Repositories.Interfaces;
using Data.Services.Interfaces;

namespace Business.UseCases.ImageContent;

public class UpdateImageContentUseCase(
    IImageContentRepository repository,
    IMediaStorageService storage
)
{
    private const string Container = "images";

    public async Task<Result<ImageContentDto>> ExecuteAsync(
        int contentId,
        UpdateImageContentDto dto,
        Stream? fileStream,
        string? fileName,
        long? fileSize
    )
    {
        var existing = await repository.GetByContentIdAsync(contentId);

        if (existing is null)
            return Result<ImageContentDto>.Failure(["Imagen no encontrada"]);

        // ACTUALIZAR TEXTO
        if (!string.IsNullOrWhiteSpace(dto.AltText))
        {
            existing.TextoAlternativo = dto.AltText;
        }

        // ACTUALIZAR ORDER 
        if (dto.Order.HasValue && dto.Order.Value > 0)
        {
            existing.Contenido.Orden = dto.Order.Value;
        }

        //  REEMPLAZAR IMAGEN
        if (fileStream != null && fileName != null)
        {
            if (fileStream.CanSeek)
                fileStream.Position = 0;

            var blobName = await storage.UploadAsync(fileStream, fileName, Container);

            existing.UrlImagen = blobName;

            if (fileSize.HasValue)
            {
                existing.TamanoKb = (int)(fileSize.Value / 1024);
            }
        }

        await repository.UpdateAsync(existing);

        var url = await storage.GetReadUrlAsync(
            existing.UrlImagen,
            Container,
            TimeSpan.FromHours(1)
        );

        return Result<ImageContentDto>.Success(new ImageContentDto
        {
            ContentId = existing.ContenidoId,
            ImageUrl = url.ToString(),
            Format = existing.Formato.ToString(),
            AltText = existing.TextoAlternativo,
            SizeKb = existing.TamanoKb,

            // DEVOLVER CONTENT COMPLETO
            Content = new ContentDto
            {
                Id = existing.ContenidoId,
                LessonId = existing.Contenido.LeccionId,
                Title = existing.Contenido.Titulo,
                Order = existing.Contenido.Orden
            }
        });
    }
}