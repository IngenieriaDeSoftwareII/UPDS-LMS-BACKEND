using Data.Context;
using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Implementations;

public class CourseRepository(AppDbContext context) : ICourseRepository
{
    public async Task<Course> CreateAsync(Course course)
    {
        context.Courses.Add(course);
        await context.SaveChangesAsync();
        return course;
    }

    public async Task<IEnumerable<Course>> GetAllAsync()
    {
        return await context.Courses
            .AsNoTracking()
            .Include(c => c.Categoria)
            .Include(c => c.Docente)
            .Where(c => c.EntityStatus == 1)
            .ToListAsync();
    }

    public async Task<Course?> GetByIdAsync(int id)
    {
        return await context.Courses
            .Include(c => c.Categoria)
            .Include(c => c.Docente)
            .FirstOrDefaultAsync(c => c.Id == id && c.EntityStatus == 1);
    }

    public async Task UpdateAsync(Course course)
    {
        course.UpdatedAt = DateTime.UtcNow;
        context.Courses.Update(course);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Course>> GetByTeacherIdAsync(int teacherId)
    {
        return await context.Courses
            .AsNoTracking()
            .Include(c => c.Categoria)
            .Include(c => c.Docente)
            .Where(c => c.DocenteId == teacherId && c.EntityStatus == 1)
            .ToListAsync();
    }

    public async Task<IEnumerable<Course>> GetByTeacherIdWithoutEvaluationAsync(int teacherId)
    {
        return await context.Courses
            .AsNoTracking()
            .Include(c => c.Categoria)
            .Include(c => c.Docente)
            .Where(c => c.DocenteId == teacherId && c.EntityStatus == 1)
            .Where(c => !context.Evaluations.Any(e => e.CursoId == c.Id && e.EntityStatus == 1))
            .ToListAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var course = await context.Courses
            .FirstOrDefaultAsync(c => c.Id == id && c.EntityStatus == 1);

        if (course != null)
        {
            course.EntityStatus = 0; // Soft delete
            course.DeletedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();
        }
    }
}
