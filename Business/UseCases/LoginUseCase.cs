using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Entities;
using Data.Enums;
using Data.Repositories.Interfaces;
using Data.Services.Interfaces;
using FluentValidation;

namespace Business.UseCases;

public class LoginUseCase(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IJwtTokenService jwtTokenService,
    IValidator<LoginDto> validator)
{
    public async Task<Result<AuthResponseDto>> ExecuteAsync(LoginDto dto)
    {
        var validation = await validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<AuthResponseDto>.Failure(validation.Errors.Select(e => e.ErrorMessage));

        // Verificar que el usuario exista
        var user = await userRepository.FindByEmailAsync(dto.Email);
        if (user is null)
            return Result<AuthResponseDto>.Failure(["Credenciales inválidas"]);

        // Verificar estado activo (HU-04: cuenta deshabilitada via LockoutEnd)
        if (await userRepository.IsLockedOutAsync(user))
            return Result<AuthResponseDto>.Failure(["Cuenta deshabilitada"]);

        // Verificar contraseña
        if (!await userRepository.CheckPasswordAsync(user, dto.Password))
            return Result<AuthResponseDto>.Failure(["Credenciales inválidas"]);

        // Obtener rol y generar tokens
        var roles = await userRepository.GetRolesAsync(user);
        var accessToken = jwtTokenService.GenerateAccessToken(user, roles);
        var rawRefreshToken = jwtTokenService.GenerateRefreshToken();

        await refreshTokenRepository.SaveAsync(new RefreshToken
        {
            Token = rawRefreshToken,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(jwtTokenService.RefreshTokenExpirationDays)
        });

        var role = roles.FirstOrDefault() ?? string.Empty;

        return Result<AuthResponseDto>.Success(new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = rawRefreshToken,
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