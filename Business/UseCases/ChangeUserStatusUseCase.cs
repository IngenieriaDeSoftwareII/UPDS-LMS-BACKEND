using Business.DTOs.Requests;
using Business.Results;
using Data.Enums;
using Data.Repositories.Interfaces;
using FluentValidation;

namespace Business.UseCases;

public class ChangeUserStatusUseCase(
    IUserRepository userRepository,
    IValidator<ChangeUserStatusDto> validator)
{
    public async Task<Result<bool>> ExecuteAsync(string userId, ChangeUserStatusDto dto)
    {
        var validation = await validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<bool>.Failure(validation.Errors.Select(e => e.ErrorMessage));

        var user = await userRepository.FindByIdWithPersonAsync(userId);
        if (user is null)
            return Result<bool>.Failure(["El usuario no existe"]);

        if (dto.IsActive && !user.Person.IsActive)
            return Result<bool>.Failure(["No se puede habilitar la cuenta de usuario porque la persona asociada se encuentra desactivada."]);

        var roles = await userRepository.GetRolesAsync(user);
        if (!dto.IsActive && roles.Contains(UserRoles.Admin))
            return Result<bool>.Failure(["No está permitido deshabilitar a un usuario con rol de Administrador."]);

        DateTimeOffset? lockoutEnd = dto.IsActive 
            ? null 
            : (dto.LockedUntil ?? DateTimeOffset.MaxValue);

        var (succeeded, errors) = await userRepository.SetLockoutEndDateAsync(user, lockoutEnd);

        if (!succeeded)
            return Result<bool>.Failure(errors);

        return Result<bool>.Success(true);
    }
}
