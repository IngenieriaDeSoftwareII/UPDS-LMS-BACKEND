using Data.Context;
using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Implementations;

public class GradableItemRepository(AppDbContext context) : IGradableItemRepository
{
    public async Task<IReadOnlyList<GradableItem>> GetActiveByCourseIdAsync(int cursoId)
    {
        return await context.GradableItems
            .AsNoTracking()
            .Include(g => g.Module)
            .Include(g => g.Evaluation!)
                .ThenInclude(e => e.Preguntas)
            .Where(g => g.EntityStatus == 1
                && g.Module.CursoId == cursoId
                && (g.Module.EntityStatus == null || g.Module.EntityStatus == 1))
            .OrderBy(g => g.Module.Orden)
            .ThenBy(g => g.Orden)
            .ToListAsync();
    }
}
