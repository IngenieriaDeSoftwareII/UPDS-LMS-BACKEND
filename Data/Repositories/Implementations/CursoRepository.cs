using Data.Context;
using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Implementations;

public class CursoRepository(AppDbContext context) : ICursoRepository
{
    public async Task<Curso> CreateAsync(Curso curso)
    {
        context.Cursos.Add(curso);
        await context.SaveChangesAsync();
        return curso;
    }

    public async Task<IEnumerable<Curso>> GetAllAsync()
    {
        return await context.Cursos
            .Include(c => c.Categoria)
            .Include(c => c.Docente)
            .Where(c => c.EntityStatus == 1).ToListAsync();
    }

    public async Task<Curso?> GetByIdAsync(int id)
    {
        return await context.Cursos
            .Include(c => c.Categoria)
            .Include(c => c.Docente)
            .FirstOrDefaultAsync(c => c.Id == id && c.EntityStatus == 1);
    }

    public async Task UpdateAsync(Curso curso)
    {
        curso.UpdatedAt = DateTime.UtcNow;
        context.Cursos.Update(curso);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var curso = await GetByIdAsync(id);
        if (curso != null)
        {
            curso.EntityStatus = 0; // Soft delete
            curso.DeletedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();
        }
    }
}
