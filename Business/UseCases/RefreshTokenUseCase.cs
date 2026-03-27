using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Entities;
using Data.Enums;
using Data.Repositories.Interfaces;
using Data.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Business.UseCases;

public class RefreshTokenUseCase(
    IRefreshTokenRepository refreshTokenRepository,
    IUserRepository userRepository,
    IJwtTokenService jwtTokenService,
    IMemoryCache cache,
    IConfiguration configuration)
{
    public async Task<Result<AuthResponseDto>> ExecuteAsync(RefreshTokenDto dto)
    {
        // ─── Seguridad 1: Detección de robo (Reuse Detection) ───
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

        // ─── Seguridad 2: Verificación de actividad del servidor ───
        // Rechazar el refresh si el usuario no hizo ninguna petición
        // real al backend en el tiempo configurado de inactividad.
        var inactivityMinutes = int.Parse(configuration["Jwt:AccessTokenExpirationMinutes"]!);
        var cacheKey = $"user_activity:{stored.UserId}";

        if (cache.TryGetValue<DateTime>(cacheKey, out var lastActivity))
        {
            var inactiveMinutes = (DateTime.UtcNow - lastActivity).TotalMinutes;
            if (inactiveMinutes > inactivityMinutes)
            {
                await refreshTokenRepository.RevokeAsync(stored);
                return Result<AuthResponseDto>.Failure(
                    ["Sesión expirada por inactividad. Por favor, inicie sesión nuevamente."]);
            }
        }
        // Nota: Si no hay registro en caché (ej: el servidor se reinició),
        // permitimos el refresh por gracia. La próxima petición autenticada
        // registrará la actividad y el sistema se estabiliza.

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
            ExpiresIn = jwtTokenService.AccessTokenExpirationMinutes * 60,
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