using Data.Context;
using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Implementations;

public class LessonRepository(AppDbContext context) : ILessonRepository
{
    public async Task<Lesson> CreateAsync(Lesson lesson)
    {
        context.Lessons.Add(lesson);
        await context.SaveChangesAsync();
        return lesson;
    }

    public async Task DeleteAsync(int id)
    {
        var lesson = await context.Lessons.FindAsync(id);
        if (lesson != null)
        {
            context.Lessons.Remove(lesson);
            await context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Lesson>> GetAllAsync()
    {
        return await context.Lessons.ToListAsync();
    }

    public async Task<Lesson?> GetByIdAsync(int? id)
    {
        return await context.Lessons.FindAsync(id);
    }

    public async Task<Lesson?> GetByIdWithModuleAndCourseAsync(int id)
    {
        return await context.Lessons
            .AsNoTracking()
            .Include(l => l.Modulos)
            .FirstOrDefaultAsync(l => l.Id == id && l.EntityStatus == 1);
    }

    public async Task<int> CountActiveLessonsByCourseAsync(int cursoId)
    {
        return await context.Lessons
            .AsNoTracking()
            .CountAsync(l => l.EntityStatus == 1
                && l.ModuloId != null
                && context.Modules.Any(m =>
                    m.Id == l.ModuloId
                    && m.CursoId == cursoId
                    && (m.EntityStatus == null || m.EntityStatus == 1)));
    }

    public async Task<Lesson> UpdateAsync(Lesson lesson)
    {
        context.Lessons.Update(lesson);
        await context.SaveChangesAsync();
        return lesson;
    }
}