using Data.Entities;

namespace Data.Services.Interfaces;

public interface IJwtTokenService
{
    string GenerateAccessToken(User user, IList<string> roles);
    string GenerateRefreshToken();
    int AccessTokenExpirationMinutes { get; }
    int RefreshTokenExpirationDays { get; }
}