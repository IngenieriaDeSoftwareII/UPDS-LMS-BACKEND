using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Repositories.Interfaces;
using FluentValidation;

namespace Business.UseCases;

public class UpdateUserUseCase(
    IUserRepository userRepository,
    IMapper mapper,
    IValidator<UpdateUserDto> validator)
{
    public async Task<Result<UserDto>> ExecuteAsync(string userId, UpdateUserDto dto)
    {
        var validation = await validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<UserDto>.Failure(validation.Errors.Select(e => e.ErrorMessage));

        var user = await userRepository.FindByIdWithPersonAsync(userId);
        if (user is null)
            return Result<UserDto>.Failure(["El usuario no existe"]);

        if (dto.Email is not null)
        {
            if (dto.Email != user.Email && await userRepository.EmailExistsAsync(dto.Email))
                return Result<UserDto>.Failure(["El correo electrónico especificado ya está en uso"]);
            
            user.Email = dto.Email;
            user.UserName = dto.Email;
            var (succeeded, errors) = await userRepository.UpdateAsync(user);
            if (!succeeded)
                return Result<UserDto>.Failure(errors);
        }

        if (dto.Role is not null)
        {
            var currentRoles = await userRepository.GetRolesAsync(user);
            await userRepository.RemoveRolesAsync(user, currentRoles);
            await userRepository.AssignRoleAsync(user, dto.Role);
        }

        var result = mapper.Map<UserDto>(user);
        var roles = await userRepository.GetRolesAsync(user);
        result.Role = roles.FirstOrDefault() ?? string.Empty;

        return Result<UserDto>.Success(result);
    }
}
