using Business.Results;
using Data.Context;
using Data.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Business.UseCases.DocumentContent;

public class DeleteDocumentContentUseCase
{
    private readonly AppDbContext db;
    private readonly IMediaStorageService storage;

    private const string Container = "documents";

    public DeleteDocumentContentUseCase(
        AppDbContext db,
        IMediaStorageService storage
    )
    {
        this.db = db;
        this.storage = storage;
    }

    public async Task<Result<bool>> ExecuteAsync(int contentId)
    {
        // 1. Buscar Content
        var content = await db.Contents
            .FirstOrDefaultAsync(c => c.Id == contentId);

        if (content is null)
            return Result<bool>.Failure(["Content no encontrado"]);

        //  2. Buscar DocumentContent
        var document = await db.DocumentContents
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.ContenidoId == contentId);

        //  3. Eliminar archivo de Azure
        if (document != null && !string.IsNullOrWhiteSpace(document.UrlArchivo))
        {
            try
            {
                var blobName = document.UrlArchivo;

                if (blobName.StartsWith("http"))
                {
                    var uri = new Uri(blobName);
                    blobName = Path.GetFileName(uri.AbsolutePath);
                }

                await storage.DeleteAsync(blobName, Container);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error eliminando documento: {ex.Message}");
            }
        }

        //  4. Eliminar Content (cascade elimina DocumentContent)
        db.Contents.Remove(content);

        await db.SaveChangesAsync();

        return Result<bool>.Success(true);
    }
}