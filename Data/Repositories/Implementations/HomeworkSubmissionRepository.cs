using Data.Context;
using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Implementations;

public class HomeworkSubmissionRepository(AppDbContext context) : IHomeworkSubmissionRepository
{
    public async Task<HomeworkSubmission?> GetByIdAsync(int id)
    {
        return await context.HomeworkSubmissions
            .Include(s => s.Usuario)
            .Include(s => s.Homework)
            .FirstOrDefaultAsync(s => s.Id == id && s.EntityStatus == 1);
    }

    public async Task<IEnumerable<HomeworkSubmission>> GetAllAsync()
    {
        return await context.HomeworkSubmissions
            .Include(s => s.Usuario)
            .Include(s => s.Homework)
            .Where(s => s.EntityStatus == 1)
            .ToListAsync();
    }

    public async Task<HomeworkSubmission?> GetByUserAndHomeworkAsync(int homeworkId, int usuarioId)
    {
        return await context.HomeworkSubmissions
            .Include(s => s.Usuario)
            .FirstOrDefaultAsync(s => s.HomeworkId == homeworkId && s.UsuarioId == usuarioId && s.EntityStatus == 1);
    }

    public async Task<IEnumerable<HomeworkSubmission>> GetByHomeworkIdAsync(int homeworkId)
    {
        return await context.HomeworkSubmissions
            .Include(s => s.Usuario)
            .Include(s => s.Homework)
            .Where(s => s.HomeworkId == homeworkId && s.EntityStatus == 1)
            .ToListAsync();
    }

    public async Task<HomeworkSubmission> CreateAsync(HomeworkSubmission submission)
    {
        submission.UpdatedAt = DateTime.Now;
        context.HomeworkSubmissions.Add(submission);
        await context.SaveChangesAsync();
        return submission;
    }

    public async Task<HomeworkSubmission> UpdateAsync(HomeworkSubmission submission)
    {
        submission.UpdatedAt = DateTime.Now;
        context.HomeworkSubmissions.Update(submission);
        await context.SaveChangesAsync();
        return submission;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            context.HomeworkSubmissions.Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}