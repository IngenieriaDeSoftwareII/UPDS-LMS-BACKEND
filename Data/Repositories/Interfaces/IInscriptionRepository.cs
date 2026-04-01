using Data.Entities;

namespace Data.Repositories.Interfaces
{
    public interface IInscriptionRepository
    {
        Task<Inscription?> GetByUserAndCourseAsync(int usuarioId, int cursoId);
        Task<IEnumerable<Inscription>> GetByUserAsync(int usuarioId);
        Task<IEnumerable<Course>> GetCoursesWithEvaluationsByUserAsync(int usuarioId);
        Task<Inscription> CreateAsync(Inscription inscripcion);
        Task<Inscription> UpdateAsync(Inscription inscripcion);
        Task<int> CountActiveInscriptionsByCourseAsync(int cursoId);
    }
}
