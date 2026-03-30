using Data.Entities;

namespace Data.Repositories.Interfaces;

public interface ICatalogRepository
{
    Task<Catalog> CreateAsync(Catalog catalog);
    Task<IEnumerable<Catalog>> GetAllAsync();
    Task<Catalog?> GetByIdAsync(int id);
    Task UpdateAsync(Catalog catalog);
    Task DeleteAsync(int id);
}
