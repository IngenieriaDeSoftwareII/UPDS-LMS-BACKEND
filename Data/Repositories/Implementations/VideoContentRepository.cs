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

    public async Task<VideoContent> CreateAsync(VideoContent videoContent)
    {
        _context.VideoContents.Add(videoContent);
        await _context.SaveChangesAsync();
        return videoContent;
    }

    public async Task DeleteAsync(int id)
    {
        var videoContent = await _context.VideoContents.FindAsync(id);
        if (videoContent != null)
        {
            _context.VideoContents.Remove(videoContent);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<VideoContent>> GetAllAsync()
    {
        return await _context.VideoContents.ToListAsync();
    }

    public async Task<VideoContent?> GetByIdAsync(int? id)
    {
        return await _context.VideoContents.FindAsync(id);
    }

    public async Task<VideoContent?> GetByContentIdAsync(int? contentId)
    {
        return await _context.VideoContents.FirstOrDefaultAsync(vc => vc.ContentId == contentId);
    }
    public async Task<VideoContent> UpdateAsync(VideoContent videoContent)
    {
        _context.VideoContents.Update(videoContent);
        await _context.SaveChangesAsync();
        return videoContent;
    }
}