using Data.Context;
using Data.Entities;
using Data.Enums;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Implementations
{
    public class InscriptionRepository(AppDbContext dbContext) : IInscriptionRepository
    {

        public async Task<int> CountActiveInscriptionsByCourseAsync(int cursoId)
        {
            return await dbContext.Inscriptions
                .Where(i => i.CursoId == cursoId
                    && (i.Estado == InscriptionEstado.Activo || i.Estado == InscriptionEstado.Progreso))
                .CountAsync();
        }

        public async Task<Inscription> CreateAsync(Inscription inscripcion)
        {
            dbContext.Inscriptions.Add(inscripcion);
            await dbContext.SaveChangesAsync();
            return inscripcion;
        }

        public async Task<Inscription> UpdateAsync(Inscription inscripcion)
        {
            inscripcion.UpdatedAt = DateTime.Now;
            await dbContext.SaveChangesAsync();
            return inscripcion;
        }

        public async Task<Inscription?> GetByUserAndCourseAsync(int usuarioId, int cursoId)
        {
            return await dbContext.Inscriptions
                .Where(i => i.UsuarioId == usuarioId && i.CursoId == cursoId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Inscription>> GetByUserAsync(int usuarioId)
        {
            return await dbContext.Inscriptions
                .Where(i => i.UsuarioId == usuarioId && i.Estado != InscriptionEstado.Cancelado)
                .Include(i => i.Curso)
                .ToListAsync();
        }
    }
}
