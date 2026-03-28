using Data.Entities;

namespace Data.Repositories.Interfaces;

public interface IImageContentRepository
{
    Task<ImageContent> CreateAsync(ImageContent imageContent);

    Task<ImageContent?> GetByContentIdAsync(int contentId);
    Task<IEnumerable<ImageContent>> GetAllAsync();

    Task <ImageContent>UpdateAsync(ImageContent imageContent);

    Task DeleteAsync(int contentId);
}