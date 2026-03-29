using Data.Context;
using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Implementations;

public class CatalogRepository(AppDbContext context) : ICatalogRepository
{
    public async Task<Catalog> CreateAsync(Catalog catalog)
    {
        context.Catalogs.Add(catalog);
        await context.SaveChangesAsync();
        return catalog;
    }

    public async Task<IEnumerable<Catalog>> GetAllAsync()
    {
        return await context.Catalogs
            .Include(c => c.Categorias.Where(cat => cat.EntityStatus == 1))
                .ThenInclude(cat => cat.Courses.Where(curso => curso.EntityStatus == 1))
            .Where(c => c.EntityStatus == 1)
            .ToListAsync();
    }

    public async Task<Catalog?> GetByIdAsync(int id)
    {
        return await context.Catalogs
            .Include(c => c.Categorias.Where(cat => cat.EntityStatus == 1))
                .ThenInclude(cat => cat.Courses.Where(curso => curso.EntityStatus == 1))
            .FirstOrDefaultAsync(c => c.Id == id && c.EntityStatus == 1);
    }

    public async Task UpdateAsync(Catalog catalog)
    {
        context.Catalogs.Update(catalog);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var catalog = await GetByIdAsync(id);
        if (catalog != null)
        {
            catalog.EntityStatus = 0; // Soft delete
            await context.SaveChangesAsync();
        }
    }
}