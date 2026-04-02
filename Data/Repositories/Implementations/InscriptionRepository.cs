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
                    && (i.Estado == InscriptionEstate.Activo || i.Estado == InscriptionEstate.Progreso))
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
                .Where(i => i.UsuarioId == usuarioId && i.Estado != InscriptionEstate.Cancelado)
                .Include(i => i.Cursos)
                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetCoursesWithEvaluationsByUserAsync(int usuarioId)
        {
            // Obtener cursos inscritos con evaluaciones, y filtrar donde intentos usados < intentos permitidos
            var coursesWithAttempts = await dbContext.Inscriptions
                .Where(i => i.UsuarioId == usuarioId && i.Estado != InscriptionEstate.Cancelado)
                .Join(dbContext.Evaluations,
                    i => i.CursoId,
                    e => e.CursoId,
                    (i, e) => new { Inscription = i, Evaluation = e })
                .GroupJoin(dbContext.EvaluationAttempts.Where(ea => ea.UsuarioId == usuarioId),
                    ie => ie.Evaluation.Id,
                    ea => ea.EvaluacionId,
                    (ie, attempts) => new { ie.Inscription, ie.Evaluation, Attempts = attempts })
                .Select(g => new {
                    Course = g.Inscription.Cursos,
                    Evaluation = g.Evaluation,
                    AttemptsUsed = g.Attempts.Count()
                })
                .Where(x => x.AttemptsUsed < x.Evaluation.IntentosPermitidos)
                .Select(x => x.Course)
                .Distinct()
                .Include(c => c.Categoria)
                .Include(c => c.Docente)
                .ToListAsync();

            return coursesWithAttempts;
        }
    }
}
