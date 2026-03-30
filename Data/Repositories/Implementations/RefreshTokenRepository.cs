using Data.Context;
using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Implementations;

public class RefreshTokenRepository(AppDbContext context) : IRefreshTokenRepository
{
    public async Task SaveAsync(RefreshToken refreshToken)
    {
        context.RefreshTokens.Add(refreshToken);
        await context.SaveChangesAsync();
    }

    public async Task<RefreshToken?> FindValidAsync(string token)
    {
        return await context.RefreshTokens
            .Include(r => r.User)
            .ThenInclude(u => u.Person)
            .FirstOrDefaultAsync(r =>
                r.Token == token &&
                !r.IsRevoked &&
                r.ExpiresAt > DateTime.UtcNow);
    }

    public async Task RevokeAsync(RefreshToken refreshToken)
    {
        refreshToken.IsRevoked = true;
        await context.SaveChangesAsync();
    }

    public async Task<RefreshToken?> FindRevokedAsync(string token)
    {
        return await context.RefreshTokens
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == token && r.IsRevoked);
    }

    public async Task RevokeAllByUserAsync(string userId)
    {
        var tokens = await context.RefreshTokens
            .Where(r => r.UserId == userId && !r.IsRevoked)
            .ToListAsync();

        foreach (var token in tokens)
            token.IsRevoked = true;

        await context.SaveChangesAsync();
    }
}