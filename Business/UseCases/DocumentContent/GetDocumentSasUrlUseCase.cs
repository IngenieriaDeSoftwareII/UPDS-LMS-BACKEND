using Data.Services.Interfaces;

namespace Business.UseCases.DocumentContent;

public class GetDocumentSasUrlUseCase
{
    private readonly IMediaStorageService _mediaStorage;

    public GetDocumentSasUrlUseCase(IMediaStorageService mediaStorage)
    {
        _mediaStorage = mediaStorage;
    }

    public async Task<Uri> ExecuteAsync(string blobName, string containerName, TimeSpan expiry)
    {
        return await _mediaStorage.GetReadUrlAsync(blobName, containerName, expiry);
    }
}