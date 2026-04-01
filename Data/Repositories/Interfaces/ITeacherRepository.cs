using Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories.Interfaces;

public interface ITeacherRepository
{
    Task<Teacher> CreateAsync(Teacher teacher);
    Task<IEnumerable<Teacher>> GetAllAsync();
    Task<Teacher?> GetByIdAsync(int id);
    Task<Teacher?> GetByUserIdAsync(string userId);
    Task UpdateAsync(Teacher teacher);
    Task DeleteAsync(int id);
}