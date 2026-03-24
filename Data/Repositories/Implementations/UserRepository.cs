using Data.Context;
using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Implementations;

public class UserRepository(UserManager<User> userManager, AppDbContext context) : IUserRepository
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

    public async Task<IEnumerable<User>> GetAllAsync(string? search = null)
    {
        var query = context.Users
            .Include(u => u.Person)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.ToLower();
            query = query.Where(u =>
                (u.Person.FirstName + " " + u.Person.LastName).ToLower().Contains(term) ||
                u.Email!.ToLower().Contains(term));
        }

        return await query.ToListAsync();
    }

    public async Task<User?> FindByEmailAsync(string email)
    {
        return await userManager.FindByEmailAsync(email);
    }

    public async Task<bool> CheckPasswordAsync(User user, string password)
    {
        return await userManager.CheckPasswordAsync(user, password);
    }

    public async Task<bool> IsLockedOutAsync(User user)
    {
        return await userManager.IsLockedOutAsync(user);
    }

    public async Task<IList<string>> GetRolesAsync(User user)
    {
        return await userManager.GetRolesAsync(user);
    }
}