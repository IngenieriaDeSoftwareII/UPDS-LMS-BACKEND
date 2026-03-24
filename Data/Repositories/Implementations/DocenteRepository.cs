using Data.Context;
using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Implementations;

public class DocenteRepository(AppDbContext context) : IDocenteRepository
{
    public async Task<Docente> CreateAsync(Docente docente)
    {
        context.Docentes.Add(docente);
        await context.SaveChangesAsync();
        return docente;
    }

    public async Task<IEnumerable<Docente>> GetAllAsync()
    {
        return await context.Docentes
            .Include(d => d.User)
            .Where(d => d.EntityStatus == 1).ToListAsync();
    }

    public async Task<Docente?> GetByIdAsync(int id)
    {
        return await context.Docentes
            .Include(d => d.User)
            .FirstOrDefaultAsync(d => d.Id == id && d.EntityStatus == 1);
    }

    public async Task UpdateAsync(Docente docente)
    {
        context.Docentes.Update(docente);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var docente = await GetByIdAsync(id);
        if (docente != null)
        {
            docente.EntityStatus = 0; // Soft delete
            await context.SaveChangesAsync();
        }
    }
}
