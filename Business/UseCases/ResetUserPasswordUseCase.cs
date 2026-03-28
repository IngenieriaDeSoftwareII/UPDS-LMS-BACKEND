using Business.DTOs.Responses;
using Business.Results;
using Data.Repositories.Interfaces;

namespace Business.UseCases;

public class ResetUserPasswordUseCase(IUserRepository userRepository)
{
    public async Task<Result<PasswordResetDto>> ExecuteAsync(string userId)
    {
        var user = await userRepository.FindByIdAsync(userId);
        if (user is null)
            return Result<PasswordResetDto>.Failure(["El usuario no existe"]);

        var newPassword = GenerateTemporaryPassword();

        var (succeeded, errors) = await userRepository.ResetPasswordAsync(user, newPassword);
        if (!succeeded)
            return Result<PasswordResetDto>.Failure(errors);

        return Result<PasswordResetDto>.Success(new PasswordResetDto
        {
            UserId = user.Id,
            TemporaryPassword = newPassword
        });
    }

    private static string GenerateTemporaryPassword()
    {
        var suffix = Random.Shared.Next(100, 999);
        return $"Upds@{DateTime.UtcNow.Year}!{suffix}";
    }
}
