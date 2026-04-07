using Data.Entities;

namespace Data.Repositories.Interfaces;

public interface IVideoContentRepository
{
    Task<VideoContent> CreateAsync(VideoContent videoContent);

    Task<VideoContent?> GetByContentIdAsync(int? contentId);

    Task <VideoContent>UpdateAsync(VideoContent videoContent);

    Task<List<VideoContent>> GetAllAsync();
    
    Task<VideoContent?> GetByIdAsync(int? id);

    Task DeleteAsync(int contentId);
}