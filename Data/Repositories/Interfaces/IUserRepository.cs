using Data.Entities;

namespace Data.Repositories.Interfaces;

public interface IUserRepository
{
    Task<bool> EmailExistsAsync(string email);
    Task<(bool Succeeded, IEnumerable<string> Errors)> CreateAsync(User user, string password);
    Task AssignRoleAsync(User user, string role);
    Task<IEnumerable<User>> GetAllAsync(string? search = null);
    Task<IList<string>> GetRolesAsync(User user);
    Task<User?> FindByEmailAsync(string email);
    Task<bool> CheckPasswordAsync(User user, string password);
    Task<bool> IsLockedOutAsync(User user);
}