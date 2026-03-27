using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Entities;
using Data.Repositories.Interfaces;
using FluentValidation;

namespace Business.UseCases;

public class CreateUserUseCase(
    IPersonRepository personRepository,
    IUserRepository userRepository,
    IValidator<CreateUserDto> validator)
{
    public async Task<Result<UserCreatedDto>> ExecuteAsync(CreateUserDto dto)
    {
        var validation = await validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<UserCreatedDto>.Failure(validation.Errors.Select(e => e.ErrorMessage));

        if (await userRepository.EmailExistsAsync(dto.Email))
            return Result<UserCreatedDto>.Failure(["Este correo ya está registrado"]);

        var person = await personRepository.GetByIdAsync(dto.PersonId);
        if (person is null)
            return Result<UserCreatedDto>.Failure(["La persona especificada no existe"]);

        if (!person.IsActive)
            return Result<UserCreatedDto>.Failure(["La persona seleccionada ha sido desactivada y no se le puede crear un usuario"]);


        var tempPassword = GenerateTemporaryPassword();
        var user = new User
        {
            UserName = dto.Email,
            Email = dto.Email,
            PersonId = person.Id
        };

        var (succeeded, errors) = await userRepository.CreateAsync(user, tempPassword);
        if (!succeeded)
            return Result<UserCreatedDto>.Failure(errors);


        await userRepository.AssignRoleAsync(user, dto.Role);

        return Result<UserCreatedDto>.Success(new UserCreatedDto
        {
            Id = user.Id,
            FullName = $"{person.FirstName} {person.LastName}",
            Email = dto.Email,
            Role = dto.Role,
            TemporaryPassword = tempPassword
        });
    }


    private static string GenerateTemporaryPassword()
    {
        var suffix = Random.Shared.Next(100, 999);
        return $"Upds@{DateTime.UtcNow.Year}!{suffix}";
    }
}