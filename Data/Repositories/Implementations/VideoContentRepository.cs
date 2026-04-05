using Data.Context;
using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Implementations;

public class VideoContentRepository : IVideoContentRepository
{
    private readonly AppDbContext _context;

    public VideoContentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<VideoContent> CreateAsync(VideoContent video)
    {
        _context.VideoContents.Add(video);
        await _context.SaveChangesAsync();
        return video;
    }

    public async Task<IEnumerable<VideoContent>> GetAllAsync()
    {
        return await _context.VideoContents
            .Include(v => v.Contenido)
            .ToListAsync();
    }

    public async Task<VideoContent?> GetByContentIdAsync(int contentId)
    {
        return await _context.VideoContents
            .Include(v => v.Contenido)
            .FirstOrDefaultAsync(v => v.ContenidoId == contentId);
    }

    public async Task<VideoContent> UpdateAsync(VideoContent video)
    {
        _context.VideoContents.Update(video);
        await _context.SaveChangesAsync();
        return video;
    }

    public async Task DeleteAsync(int contentId)
    {
        var video = await GetByContentIdAsync(contentId);

        if (video != null)
        {
            _context.VideoContents.Remove(video);
            await _context.SaveChangesAsync();
        }
    }
    public async Task<IEnumerable<VideoContent>> GetAllWithContentAsync()
    {
        return await _context.VideoContents
            .Include(v => v.Contenido) 
            .ToListAsync();
    }
}