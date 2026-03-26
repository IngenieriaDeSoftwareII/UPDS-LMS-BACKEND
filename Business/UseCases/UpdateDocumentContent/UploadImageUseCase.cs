using Data.Services.Interfaces;

namespace Business.UseCases.UpdateDocumentContent;

public class UploadImageUseCase(IMediaStorageService storage)
{
    private const string Container = "images";
    private static readonly TimeSpan UrlExpiry = TimeSpan.FromHours(1);

    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".gif", ".webp"
    };

    public async Task<Uri> ExecuteAsync(Stream imageStream, string fileName)
    {
        var extension = Path.GetExtension(fileName);
        if (!AllowedExtensions.Contains(extension))
            throw new InvalidOperationException($"Tipo de archivo no permitido: {extension}");

        var blobName = await storage.UploadAsync(imageStream, fileName, Container);
        return await storage.GetReadUrlAsync(blobName, Container, UrlExpiry);
    }
}
