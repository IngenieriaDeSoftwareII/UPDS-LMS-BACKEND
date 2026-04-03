using Data.Context;
using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Implementations;

public class HomeworkRepository(AppDbContext context) : IHomeworkRepository
{
    public async Task<Homework?> GetByIdAsync(int id)
    {
        return await context.Homeworks
            .FirstOrDefaultAsync(h => h.Id == id && h.EntityStatus == 1);
    }

    public async Task<IEnumerable<Homework>> GetAllAsync()
    {
        return await context.Homeworks
            .Where(h => h.EntityStatus == 1)
            .ToListAsync();
    }

    public async Task<IEnumerable<Homework>> GetByLessonIdAsync(int lessonId)
    {
        return await context.Homeworks
            .Where(h => h.LessonId == lessonId && h.EntityStatus == 1)
            .ToListAsync();
    }

    public async Task<Homework> CreateAsync(Homework homework)
    {
        context.Homeworks.Add(homework);
        await context.SaveChangesAsync();
        return homework;
    }

    public async Task<Homework> UpdateAsync(Homework homework)
    {
        homework.UpdatedAt = DateTime.Now;
        context.Homeworks.Update(homework);
        await context.SaveChangesAsync();
        return homework;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            context.Homeworks.Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}