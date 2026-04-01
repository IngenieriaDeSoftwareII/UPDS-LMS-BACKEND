using Data.Context;
using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Implementations;

public class ImageContentRepository : IImageContentRepository
{
    private readonly AppDbContext _context;

    public ImageContentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ImageContent> CreateAsync(ImageContent imageContent)
    {
        _context.ImageContents.Add(imageContent);
        await _context.SaveChangesAsync();
        return imageContent;
    }

    public async Task DeleteAsync(int id)
    {
        var imageContent = await _context.ImageContents
            .Include(ic => ic.Contenido) 
            .FirstOrDefaultAsync(ic => ic.ContenidoId == id);

        if (imageContent != null)
        {
            _context.ImageContents.Remove(imageContent);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<ImageContent>> GetAllAsync()
    {
        return await _context.ImageContents
            .Include(ic => ic.Contenido) 
            .ToListAsync();
    }

    public async Task<ImageContent?> GetByIdAsync(int? id)
    {
        return await _context.ImageContents
            .Include(ic => ic.Contenido) 
            .FirstOrDefaultAsync(ic => ic.ContenidoId == id);
    }

    public async Task<ImageContent?> GetByContentIdAsync(int contenidoId)
    {
        return await _context.ImageContents
            .Include(ic => ic.Contenido) 
            .FirstOrDefaultAsync(ic => ic.ContenidoId == contenidoId);
    }
    
    public async Task<ImageContent> UpdateAsync(ImageContent imageContent)
    {
        _context.ImageContents.Update(imageContent);
        await _context.SaveChangesAsync();
        return imageContent;
    }
}