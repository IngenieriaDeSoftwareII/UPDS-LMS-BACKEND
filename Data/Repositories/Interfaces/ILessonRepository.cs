using Data.Entities;

namespace Data.Repositories.Interfaces;

public interface ILessonRepository
{
    Task<Lesson> CreateAsync(Lesson lesson);
    Task<Lesson> UpdateAsync(Lesson lesson);
    Task DeleteAsync(int id);   
    Task<IEnumerable<Lesson>> GetAllAsync();
    Task<Lesson?> GetByIdAsync(int? id);
    Task<Lesson?> GetByIdWithModuleAndCourseAsync(int id);
    Task<int> CountActiveLessonsByCourseAsync(int cursoId);
}