using Business.Results;
using Data.Context;
using Data.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Business.UseCases.VideoContent;

public class DeleteVideoContentUseCase
{
    private readonly AppDbContext db;
    private readonly IMediaStorageService storage;

    private const string Container = "videos";

    public DeleteVideoContentUseCase(
        AppDbContext db,
        IMediaStorageService storage
    )
    {
        this.db = db;
        this.storage = storage;
    }

    public async Task<Result<bool>> ExecuteAsync(int contentId)
    {
        // 🔍 1. Buscar video por ContentId
        var video = await db.VideoContents
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.ContenidoId == contentId);

        if (video is null)
            return Result<bool>.Failure(["Video no encontrado"]);

        var blobName = video.UrlVideo;

        // 🧠 2. Eliminar archivo de Azure
        if (!string.IsNullOrWhiteSpace(blobName))
        {
            try
            {
                if (blobName.StartsWith("http"))
                {
                    var uri = new Uri(blobName);
                    blobName = Path.GetFileName(uri.AbsolutePath);
                }

                await storage.DeleteAsync(blobName, Container);
            }
            catch (Exception ex)
            {
                // No rompemos el flujo si falla Azure
                Console.WriteLine($"Error eliminando video: {ex.Message}");
            }
        }

        // 🔥 3. Eliminar SOLO el Content
        var content = await db.Contents
            .FirstOrDefaultAsync(c => c.Id == contentId);

        if (content is null)
            return Result<bool>.Failure(["Content no encontrado"]);

        db.Contents.Remove(content);

        await db.SaveChangesAsync();

        return Result<bool>.Success(true);
    }
}