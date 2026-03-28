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

    public async Task<User?> FindByIdAsync(string id)
    {
        return await userManager.FindByIdAsync(id);
    }

    public async Task<(bool Succeeded, IEnumerable<string> Errors)> ResetPasswordAsync(User user, string newPassword)
    {
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var result = await userManager.ResetPasswordAsync(user, token, newPassword);
        return (result.Succeeded, result.Errors.Select(e => e.Description));
    }

    public async Task<User?> FindByIdWithPersonAsync(string id)
    {
        return await context.Users
            .Include(u => u.Person)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<(bool Succeeded, IEnumerable<string> Errors)> UpdateAsync(User user)
    {
        var result = await userManager.UpdateAsync(user);
        return (result.Succeeded, result.Errors.Select(e => e.Description));
    }

    public async Task RemoveRolesAsync(User user, IList<string> roles)
    {
        await userManager.RemoveFromRolesAsync(user, roles);
    }

    public async Task<(bool Succeeded, IEnumerable<string> Errors)> SetLockoutEndDateAsync(User user, DateTimeOffset? lockoutEnd)
    {
        if (!await userManager.GetLockoutEnabledAsync(user))
            await userManager.SetLockoutEnabledAsync(user, true);

        var result = await userManager.SetLockoutEndDateAsync(user, lockoutEnd);
        return (result.Succeeded, result.Errors.Select(e => e.Description));
    }

    public async Task<(bool Succeeded, IEnumerable<string> Errors)> ChangePasswordAsync(User user, string currentPassword, string newPassword)
    {
        var result = await userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        return (result.Succeeded, result.Errors.Select(e => e.Description));
    }

    public async Task<bool> HasActiveUsersAsync(int personId)
    {
        return await context.Users
            .AnyAsync(u => u.PersonId == personId && (u.LockoutEnd == null || u.LockoutEnd <= DateTimeOffset.UtcNow));
    }
}