using Business.DTOs.Requests;
using Business.Results;
using Data.Repositories.Interfaces;
using FluentValidation;

namespace Business.UseCases;

public class ChangePasswordUseCase(
    IUserRepository userRepository,
    IValidator<ChangePasswordDto> validator)
{
    public async Task<Result<bool>> ExecuteAsync(string userId, ChangePasswordDto dto)
    {
        var validation = await validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<bool>.Failure(validation.Errors.Select(e => e.ErrorMessage));

        var user = await userRepository.FindByIdAsync(userId);
        if (user is null)
            return Result<bool>.Failure(["Usuario no encontrado"]);

        var (succeeded, errors) = await userRepository.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);

        if (!succeeded)
            return Result<bool>.Failure(errors.Select(TranslateIdentityError));

        return Result<bool>.Success(true);
    }

    private static string TranslateIdentityError(string identityError)
    {
        if (identityError.Contains("PasswordMismatch"))
            return "La contraseña actual es incorrecta";

        return identityError;
    }
}
