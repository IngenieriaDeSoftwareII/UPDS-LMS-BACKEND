using Data.Entities;

namespace Data.Repositories.Interfaces;

public interface IHomeworkRepository
{
    Task<Homework?> GetByIdAsync(int id);
    Task<IEnumerable<Homework>> GetByLessonIdAsync(int lessonId);
    Task<IEnumerable<Homework>> GetAllAsync();
    Task<Homework> CreateAsync(Homework homework);
    Task<Homework> UpdateAsync(Homework homework);
    Task DeleteAsync(int id);
    Task<IEnumerable<Homework>> GetHomeworkWithSubmissionsAsync(int homeworkId);
}