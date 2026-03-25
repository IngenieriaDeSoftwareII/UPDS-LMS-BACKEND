using Data.Entities;

namespace Data.Repositories.Interfaces;

public interface IRefreshTokenRepository
{
    Task SaveAsync(RefreshToken refreshToken);
    Task<RefreshToken?> FindValidAsync(string token);
    Task RevokeAsync(RefreshToken refreshToken);
}