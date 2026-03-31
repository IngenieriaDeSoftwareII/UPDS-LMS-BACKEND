using Data.Entities;

namespace Data.Repositories.Interfaces
{
    public interface IModuleRepository
    {
        Task<List<Module>> GetAllAsync();
        Task<Module?> GetByIdAsync(int id);
        Task<Module> AddAsync(Module module);
        Task UpdateAsync(Module module);
        Task DeleteAsync(Module module);
    }
}