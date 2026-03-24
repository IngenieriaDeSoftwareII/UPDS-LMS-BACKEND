using Data.Context;
using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore; 
namespace Data.Repositories.Implementations;

public class DocumentContentRepository : IDocumentContentRepository
{
    private readonly AppDbContext _context;

    public DocumentContentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DocumentContent> CreateAsync(DocumentContent documentContent)
    {
        _context.DocumentContents.Add(documentContent);
        await _context.SaveChangesAsync();
        return documentContent;
    }

    public async Task DeleteAsync(int contentId)
    {
        var documentContent = await _context.DocumentContents.FindAsync(contentId);
        if (documentContent != null)
        {
            _context.DocumentContents.Remove(documentContent);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<DocumentContent>> GetAllAsync()
    {
        return await _context.DocumentContents.ToListAsync();
    }

    public async Task<DocumentContent?> GetByContentIdAsync(int contentId)
    {
        return await _context.DocumentContents.FirstOrDefaultAsync(dc => dc.ContentId == contentId);
    }

    public async Task<DocumentContent> UpdateAsync(DocumentContent documentContent)
    {
        _context.DocumentContents.Update(documentContent);
        await _context.SaveChangesAsync();
        return documentContent;
    }
}