using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Entities;
using Data.Enums;
using Data.Repositories.Interfaces;
using Data.Services.Interfaces;

namespace Business.UseCases;

public class RefreshTokenUseCase(
    IRefreshTokenRepository refreshTokenRepository,
    IUserRepository userRepository,
    IJwtTokenService jwtTokenService)
{
    public async Task<Result<AuthResponseDto>> ExecuteAsync(RefreshTokenDto dto)
    {
        var stored = await refreshTokenRepository.FindValidAsync(dto.RefreshToken);
        if (stored is null)
            return Result<AuthResponseDto>.Failure(["El refresh token es inválido o ha expirado"]);

        // Verificar que el usuario siga activo
        if (await userRepository.IsLockedOutAsync(stored.User))
            return Result<AuthResponseDto>.Failure(["Cuenta deshabilitada"]);

        // Rotar: revocar el anterior y emitir uno nuevo
        await refreshTokenRepository.RevokeAsync(stored);

        var roles = await userRepository.GetRolesAsync(stored.User);
        var newAccessToken = jwtTokenService.GenerateAccessToken(stored.User, roles);
        var newRawRefreshToken = jwtTokenService.GenerateRefreshToken();

        await refreshTokenRepository.SaveAsync(new RefreshToken
        {
            Token = newRawRefreshToken,
            UserId = stored.UserId,
            ExpiresAt = DateTime.UtcNow.AddDays(jwtTokenService.RefreshTokenExpirationDays)
        });

        var role = roles.FirstOrDefault() ?? string.Empty;

        return Result<AuthResponseDto>.Success(new AuthResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRawRefreshToken,
            Role = role,
            RedirectTo = GetDashboard(role)
        });
    }

    private static string GetDashboard(string role) => role switch
    {
        UserRoles.Admin => "/admin/dashboard",
        UserRoles.Docente => "/docente/dashboard",
        UserRoles.Estudiante => "/estudiante/dashboard",
        _ => "/dashboard"
    };
}