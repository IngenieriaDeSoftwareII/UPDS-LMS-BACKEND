using Data.Entities;

namespace Data.Repositories.Interfaces;

public interface ICursoRepository
{
    Task<Curso> CreateAsync(Curso curso);
    Task<IEnumerable<Curso>> GetAllAsync();
    Task<Curso?> GetByIdAsync(int id);
    Task UpdateAsync(Curso curso);
    Task DeleteAsync(int id);
}
