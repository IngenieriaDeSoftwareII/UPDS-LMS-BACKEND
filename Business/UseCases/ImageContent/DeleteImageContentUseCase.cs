using Business.Results;
using Data.Context;
using Data.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Business.UseCases.ImageContent;

public class DeleteImageContentUseCase
{
    private readonly AppDbContext db;
    private readonly IMediaStorageService storage;

    private const string Container = "images";

    public DeleteImageContentUseCase(
        AppDbContext db,
        IMediaStorageService storage
    )
    {
        this.db = db;
        this.storage = storage;
    }

    public async Task<Result<bool>> ExecuteAsync(int contentId)
    {
        //  1. Buscar Content
        var content = await db.Contents
            .FirstOrDefaultAsync(c => c.Id == contentId);

        if (content is null)
            return Result<bool>.Failure(["Content no encontrado"]);

        // 2. Buscar ImageContent
        var image = await db.ImageContents
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.ContenidoId == contentId);

        // 3. Eliminar archivo de Azure
        if (image != null && !string.IsNullOrWhiteSpace(image.UrlImagen))
        {
            try
            {
                var blobName = image.UrlImagen;

                if (blobName.StartsWith("http"))
                {
                    var uri = new Uri(blobName);
                    blobName = Path.GetFileName(uri.AbsolutePath);
                }

                await storage.DeleteAsync(blobName, Container);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error eliminando imagen: {ex.Message}");
            }
        }

        //  4. Eliminar Content (cascade elimina ImageContent)
        db.Contents.Remove(content);

        await db.SaveChangesAsync();

        return Result<bool>.Success(true);
    }
}