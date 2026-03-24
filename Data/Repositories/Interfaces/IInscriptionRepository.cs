using Data.Entities;

namespace Data.Repositories.Interfaces
{
    public interface IInscriptionRepository
    {
        Task<Inscription?> GetByUserAndCourseAsync(int usuarioId, int cursoId);
        Task<IEnumerable<Inscription>> GetByUserAsync(int usuarioId);
        Task<Inscription> CreateAsync(Inscription inscripcion);
        Task<int> CountByCourseAsync(int cursoId);
    }
}
