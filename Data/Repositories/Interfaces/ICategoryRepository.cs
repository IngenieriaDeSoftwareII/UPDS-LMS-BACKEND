using Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<Category> CreateAsync(Category category);
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task UpdateAsync(Category category);
    Task DeleteAsync(int id);
}
