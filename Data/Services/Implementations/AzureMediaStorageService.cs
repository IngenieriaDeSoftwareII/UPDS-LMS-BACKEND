using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Data.Services.Interfaces;

namespace Data.Services.Implementations;

public class AzureMediaStorageService(BlobServiceClient blobServiceClient) : IMediaStorageService, IStorageService
{
    private static readonly Dictionary<string, string> ContentTypeMap = new(StringComparer.OrdinalIgnoreCase)
    {
        { ".pdf",  "application/pdf" }, // 🔥 IMPORTANTE
        { ".mp4",  "video/mp4" },
        { ".webm", "video/webm" },
        { ".mov",  "video/quicktime" },
        { ".avi",  "video/x-msvideo" },
        { ".mkv",  "video/x-matroska" },
        { ".mp3",  "audio/mpeg" },
        { ".ogg",  "audio/ogg" },
        { ".wav",  "audio/wav" },
        { ".jpg",  "image/jpeg" },
        { ".jpeg", "image/jpeg" },
        { ".png",  "image/png" },
        { ".gif",  "image/gif" },
        { ".webp", "image/webp" },
    };

    public async Task<string> UploadAsync(Stream content, string fileName, string containerName)
    {
        var container = await GetOrCreateContainerAsync(containerName);

        var extension = Path.GetExtension(fileName);
        var blobName = $"{Guid.NewGuid()}{extension}";

        var blob = container.GetBlobClient(blobName);

        var contentType = ContentTypeMap.GetValueOrDefault(extension, "application/octet-stream");

        var uploadOptions = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders { ContentType = contentType }
        };

        await blob.UploadAsync(content, uploadOptions);

        return blobName;
    }

    public async Task<Uri> GetReadUrlAsync(string blobName, string containerName, TimeSpan expiry)
    {
        var container = blobServiceClient.GetBlobContainerClient(containerName);
        var blob = container.GetBlobClient(blobName);

        var sasUri = blob.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.Add(expiry));

        return await Task.FromResult(sasUri);
    }

    public async Task DeleteAsync(string blobName, string containerName)
    {
        var container = blobServiceClient.GetBlobContainerClient(containerName);
        var blob = container.GetBlobClient(blobName);
        await blob.DeleteIfExistsAsync();
    }

    public async Task<bool> ExistsAsync(string blobName, string containerName)
    {
        var container = blobServiceClient.GetBlobContainerClient(containerName);
        var blob = container.GetBlobClient(blobName);
        var response = await blob.ExistsAsync();
        return response.Value;
    }

    private async Task<BlobContainerClient> GetOrCreateContainerAsync(string containerName)
    {
        var container = blobServiceClient.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync();
        return container;
    }

    // Implementación de IStorageService
    async Task<string> IStorageService.UploadFileAsync(Stream stream, string fileName, string containerName)
    {
        return await UploadAsync(stream, fileName, containerName);
    }

    async Task IStorageService.DeleteFileAsync(string fileUrl, string containerName)
    {
        await DeleteAsync(fileUrl, containerName);
    }

    async Task<Uri> IStorageService.GetReadUrlAsync(string blobName, string containerName, TimeSpan expiry)
    {
        return await GetReadUrlAsync(blobName, containerName, expiry);
    }

    Uri IStorageService.GetPublicUrl(string blobName, string containerName)
    {
        var container = blobServiceClient.GetBlobContainerClient(containerName);
        var blob = container.GetBlobClient(blobName);
        return blob.Uri;
    }
}