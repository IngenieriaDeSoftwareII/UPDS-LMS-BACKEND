using Data.Context;
using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories.Implementations;

public class TeacherRepository(AppDbContext context) : ITeacherRepository
{
    public async Task<Teacher> CreateAsync(Teacher teacher)
    {
        context.Teachers.Add(teacher);
        await context.SaveChangesAsync();
        return teacher;
    }

    public async Task<IEnumerable<Teacher>> GetAllAsync()
    {
        return await context.Teachers
            .Include(t => t.Usuario).ThenInclude(u => u.Person)
            .Include(t => t.Cursos.Where(c => c.EntityStatus == 1))
            .Where(t => t.EntityStatus == 1).ToListAsync();
    }

    public async Task<Teacher?> GetByIdAsync(int id)
    {
        return await context.Teachers
            .Include(t => t.Usuario).ThenInclude(u => u.Person)
            .Include(t => t.Cursos.Where(c => c.EntityStatus == 1))
            .FirstOrDefaultAsync(t => t.Id == id && t.EntityStatus == 1);
    }

    public async Task UpdateAsync(Teacher teacher)
    {
        context.Teachers.Update(teacher);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var teacher = await GetByIdAsync(id);
        if (teacher != null)
        {
            teacher.EntityStatus = 0; // Soft delete
            await context.SaveChangesAsync();
        }
    }
}