using Data.Entities;

namespace Data.Repositories.Interfaces;

public interface IGradableItemRepository
{
    Task<IReadOnlyList<GradableItem>> GetActiveByCourseIdAsync(int cursoId);
}
