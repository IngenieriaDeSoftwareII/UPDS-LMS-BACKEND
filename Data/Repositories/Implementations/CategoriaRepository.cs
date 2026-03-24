using Data.Context;
using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Implementations;

public class CategoriaRepository(AppDbContext context) : ICategoriaRepository
{
    public async Task<Categoria> CreateAsync(Categoria categoria)
    {
        context.Categorias.Add(categoria);
        await context.SaveChangesAsync();
        return categoria;
    }

    public async Task<IEnumerable<Categoria>> GetAllAsync()
    {
        return await context.Categorias.Where(c => c.EntityStatus == 1).ToListAsync();
    }

    public async Task<Categoria?> GetByIdAsync(int id)
    {
        return await context.Categorias.FirstOrDefaultAsync(c => c.Id == id && c.EntityStatus == 1);
    }

    public async Task UpdateAsync(Categoria categoria)
    {
        context.Categorias.Update(categoria);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var cat = await GetByIdAsync(id);
        if (cat != null)
        {
            cat.EntityStatus = 0; // Soft delete
            await context.SaveChangesAsync();
        }
    }
}
