using Data.Entities;

namespace Data.Repositories.Interfaces;

public interface IContentRepository
{
    Task<Content> CreateAsync(Content content);

    Task<IEnumerable<Content>> GetAllAsync();

    Task<Content?> GetByIdAsync(int? id);

    Task<IEnumerable<Content>> GetByLessonIdAsync(int lessonId);

    Task<Content>UpdateAsync(Content content);

    Task DeleteAsync(int id);
}