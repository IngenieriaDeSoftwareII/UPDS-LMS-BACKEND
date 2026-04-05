using Data.Entities;

namespace Data.Repositories.Interfaces;

public interface IVideoContentRepository
{
    Task<VideoContent> CreateAsync(VideoContent video);
    Task<IEnumerable<VideoContent>> GetAllAsync();
    Task<VideoContent?> GetByContentIdAsync(int contentId);
    Task<VideoContent> UpdateAsync(VideoContent video);
    Task DeleteAsync(int contentId);
    Task<IEnumerable<VideoContent>> GetAllWithContentAsync();
}