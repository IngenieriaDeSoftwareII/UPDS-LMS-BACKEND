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
    Task<User?> FindByIdAsync(string id);
    Task<User?> FindByIdWithPersonAsync(string id);
    Task<(bool Succeeded, IEnumerable<string> Errors)> UpdateAsync(User user);
    Task RemoveRolesAsync(User user, IList<string> roles);
    Task<(bool Succeeded, IEnumerable<string> Errors)> ResetPasswordAsync(User user, string newPassword);
    Task<(bool Succeeded, IEnumerable<string> Errors)> SetLockoutEndDateAsync(User user, DateTimeOffset? lockoutEnd);
    Task<(bool Succeeded, IEnumerable<string> Errors)> ChangePasswordAsync(User user, string currentPassword, string newPassword);
    Task<bool> HasActiveUsersAsync(int personId);
}