using Data.Context;
using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Implementations;

public class CatalogoRepository(AppDbContext context) : ICatalogoRepository
{
    public async Task<Catalogo> CreateAsync(Catalogo catalogo)
    {
        context.Catalogos.Add(catalogo);
        await context.SaveChangesAsync();
        return catalogo;
    }

    public async Task<IEnumerable<Catalogo>> GetAllAsync()
    {
        return await context.Catalogos
            .Include(c => c.Categorias.Where(cat => cat.EntityStatus == 1))
                .ThenInclude(cat => cat.Cursos.Where(curso => curso.EntityStatus == 1))
            .Where(c => c.EntityStatus == 1)
            .ToListAsync();
    }

    public async Task<Catalogo?> GetByIdAsync(int id)
    {
        return await context.Catalogos
            .Include(c => c.Categorias.Where(cat => cat.EntityStatus == 1))
                .ThenInclude(cat => cat.Cursos.Where(curso => curso.EntityStatus == 1))
            .FirstOrDefaultAsync(c => c.Id == id && c.EntityStatus == 1);
    }

    public async Task UpdateAsync(Catalogo catalogo)
    {
        context.Catalogos.Update(catalogo);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var catalogo = await GetByIdAsync(id);
        if (catalogo != null)
        {
            catalogo.EntityStatus = 0; // Soft delete
            await context.SaveChangesAsync();
        }
    }
}