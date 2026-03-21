namespace Data.Services.Interfaces;

public interface IMediaStorageService
{
    Task<string> UploadAsync(Stream content, string fileName, string containerName);
    Task<Uri> GetReadUrlAsync(string blobName, string containerName, TimeSpan expiry);
    Task DeleteAsync(string blobName, string containerName);
    Task<bool> ExistsAsync(string blobName, string containerName);
}
