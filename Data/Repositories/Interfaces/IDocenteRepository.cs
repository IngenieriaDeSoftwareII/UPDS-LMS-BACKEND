using Data.Entities;

namespace Data.Repositories.Interfaces;

public interface IDocenteRepository
{
    Task<Docente> CreateAsync(Docente docente);
    Task<IEnumerable<Docente>> GetAllAsync();
    Task<Docente?> GetByIdAsync(int id);
    Task UpdateAsync(Docente docente);
    Task DeleteAsync(int id);
}
