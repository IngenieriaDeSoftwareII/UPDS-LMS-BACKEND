using Data.Entities;
using Data.Repositories.Interfaces;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Implementations;

public class ContentRepository(AppDbContext context) : IContentRepository
{
    public async Task<Content> CreateAsync(Content content)
    {
        context.Contents.Add(content);
        await context.SaveChangesAsync();
        return content;
    }

    public async Task DeleteAsync(int id)
    {
        var content = await context.Contents.FindAsync(id);
        if (content != null)
        {
            context.Contents.Remove(content);
            await context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Content>> GetAllAsync()
    {
        return await context.Contents.ToListAsync();
    }

    public async Task<Content?> GetByIdAsync(int? id)
    {
        return await context.Contents.FindAsync(id);
    }

    public async Task<Content> UpdateAsync(Content content)
    {
        context.Contents.Update(content);
        await context.SaveChangesAsync();
        return content;
    }

    public async Task<IEnumerable<Content>> GetByLessonIdAsync(int LeccionId)
    {
        return await context.Contents.Where(c => c.LeccionId == LeccionId).ToListAsync();
    }
}