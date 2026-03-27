using Data.Entities;

namespace Data.Repositories.Interfaces;

public interface ICatalogoRepository
{
    Task<Catalogo> CreateAsync(Catalogo catalogo);
    Task<IEnumerable<Catalogo>> GetAllAsync();
    Task<Catalogo?> GetByIdAsync(int id);
    Task UpdateAsync(Catalogo catalogo);
    Task DeleteAsync(int id);
}