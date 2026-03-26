using Business.Results;
using Data.Repositories.Interfaces;

namespace Business.UseCases;

public class LogoutUseCase(IRefreshTokenRepository refreshTokenRepository)
{
    public async Task<Result<string>> ExecuteAsync(string refreshToken)
    {
        var stored = await refreshTokenRepository.FindValidAsync(refreshToken);
        if (stored is null)
            return Result<string>.Failure(["El token no es válido o ya fue revocado"]);

        await refreshTokenRepository.RevokeAsync(stored);

        return Result<string>.Success("Sesión cerrada exitosamente");
    }
}
