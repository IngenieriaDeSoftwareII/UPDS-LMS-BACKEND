using Data.Context;
using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories.Implementations;

public class CategoryRepository(AppDbContext context) : ICategoryRepository
{
    public async Task<Category> CreateAsync(Category category)
    {
        context.Categories.Add(category);
        await context.SaveChangesAsync();
        return category;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await context.Categories
            .Where(c => c.EntityStatus == 1)
            .Include(c => c.Cursos.Where(curso => curso.EntityStatus == 1))
            .ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await context.Categories
            .Include(c => c.Cursos.Where(curso => curso.EntityStatus == 1))
            .FirstOrDefaultAsync(c => c.Id == id && c.EntityStatus == 1);
    }

    public async Task UpdateAsync(Category category)
    {
        context.Categories.Update(category);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var category = await GetByIdAsync(id);
        if (category != null)
        {
            category.EntityStatus = 0; // Soft delete
            await context.SaveChangesAsync();
        }
    }
}