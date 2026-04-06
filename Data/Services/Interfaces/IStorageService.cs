namespace Data.Services.Interfaces;

public interface IStorageService
{
    Task<string> UploadFileAsync(Stream stream, string fileName, string containerName);

    Task DeleteFileAsync(string fileUrl, string containerName);

    Task<Uri> GetReadUrlAsync(string blobName, string containerName, TimeSpan expiry);

    Uri GetPublicUrl(string blobName, string containerName);
}
