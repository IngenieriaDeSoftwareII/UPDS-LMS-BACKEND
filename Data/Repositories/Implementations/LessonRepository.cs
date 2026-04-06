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
        return await context.Lessons
            .AsNoTracking()
            .Include(l => l.Modulos)
            .Include(l => l.Contenidos)
            .Include(l => l.Homeworks)
            .ToListAsync();
    }

    public async Task<Lesson?> GetByIdAsync(int? courseId)
    {
        return await context.Lessons.FindAsync(courseId);
    }

    public async Task<Lesson?> GetByIdWithModuleAndCourseAsync(int courseId)
    {
        return await context.Lessons
            .AsNoTracking()
            .Include(l => l.Modulos)
            .FirstOrDefaultAsync(l => l.Id == courseId && l.EntityStatus == 1);
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
    public async Task<IEnumerable<Lesson>> GetLessonsByCourseAndModuleAsync(int courseId, int moduleId)
    {
        return await context.Lessons
            .AsNoTracking()
            .Include(l => l.Modulos)
            .Include(l => l.Contenidos)
            .Include(l => l.Homeworks)
            .Where(l => l.EntityStatus == 1
                && l.ModuloId == moduleId
                && context.Modules.Any(m =>
                    m.Id == moduleId
                    && m.CursoId == courseId
                    && (m.EntityStatus == null || m.EntityStatus == 1)))
            .ToListAsync();
    }
}