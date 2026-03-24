using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Data.Repositories.Implementations;

public class UserRepository(UserManager<User> userManager) : IUserRepository
{
    public async Task<bool> EmailExistsAsync(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        return user is not null;
    }

    public async Task<(bool Succeeded, IEnumerable<string> Errors)> CreateAsync(User user, string password)
    {
        var result = await userManager.CreateAsync(user, password);
        return (result.Succeeded, result.Errors.Select(e => e.Description));
    }

    public async Task AssignRoleAsync(User user, string role)
    {
        await userManager.AddToRoleAsync(user, role);
    }
}
