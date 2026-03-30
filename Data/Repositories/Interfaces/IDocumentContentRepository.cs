using Data.Entities;

namespace Data.Repositories.Interfaces;

public interface IDocumentContentRepository
{
    Task<DocumentContent> CreateAsync(DocumentContent documentContent);

    Task<DocumentContent?> GetByContentIdAsync(int contentId);
    Task<IEnumerable<DocumentContent>> GetAllAsync();

    Task<DocumentContent> UpdateAsync(DocumentContent documentContent);

    Task DeleteAsync(int contentId);
}