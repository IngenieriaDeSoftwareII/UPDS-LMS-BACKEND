using Data.Entities;

namespace Data.Repositories.Interfaces;

public interface ICategoriaRepository
{
    Task<Categoria> CreateAsync(Categoria categoria);
    Task<IEnumerable<Categoria>> GetAllAsync();
    Task<Categoria?> GetByIdAsync(int id);
    Task UpdateAsync(Categoria categoria);
    Task DeleteAsync(int id);
}
