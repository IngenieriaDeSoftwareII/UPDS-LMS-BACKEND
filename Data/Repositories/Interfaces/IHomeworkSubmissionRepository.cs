using Data.Entities;

namespace Data.Repositories.Interfaces;

public interface IHomeworkSubmissionRepository
{
    Task<HomeworkSubmission?> GetByIdAsync(int id);
    Task<HomeworkSubmission?> GetByUserAndHomeworkAsync(int homeworkId, int usuarioId);
    Task<IEnumerable<HomeworkSubmission>> GetAllAsync();
    Task<IEnumerable<HomeworkSubmission>> GetByHomeworkIdAsync(int homeworkId);
    Task<HomeworkSubmission> CreateAsync(HomeworkSubmission submission);
    Task<HomeworkSubmission> UpdateAsync(HomeworkSubmission submission);
    Task DeleteAsync(int id);
}