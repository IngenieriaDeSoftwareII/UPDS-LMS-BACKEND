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
        // ─── Seguridad: Detección de robo (Reuse Detection) ───
        // Si alguien presenta un token que YA fue revocado, significa
        // que el token fue robado. Revocamos TODOS los tokens del usuario.
        var revokedToken = await refreshTokenRepository.FindRevokedAsync(dto.RefreshToken);
        if (revokedToken is not null)
        {
            await refreshTokenRepository.RevokeAllByUserAsync(revokedToken.UserId);
            return Result<AuthResponseDto>.Failure(
                ["Sesión comprometida: se detectó el uso de un token ya utilizado. Todas las sesiones han sido cerradas por seguridad."]);
        }

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
            ExpiresAt = DateTime.UtcNow.AddMinutes(jwtTokenService.RefreshTokenExpirationMinutes)
        });

        var role = roles.FirstOrDefault() ?? string.Empty;

        return Result<AuthResponseDto>.Success(new AuthResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRawRefreshToken,
            SessionTTL = jwtTokenService.RefreshTokenExpirationMinutes * 60 * 1000,
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