using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Entities;
using Data.Enums;
using Data.Repositories.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Business.UseCases;

public class CreateUserUseCase(
    IPersonRepository personRepository,
    UserManager<User> userManager,
    IValidator<CreateUserDto> validator)
{
    public async Task<Result<UserCreatedDto>> ExecuteAsync(CreateUserDto dto)
    {
        // 1. Validar el DTO
        var validation = await validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<UserCreatedDto>.Failure(validation.Errors.Select(e => e.ErrorMessage));

        // 2. Verificar que el correo no esté duplicado (HU-01 CS #1)
        var existing = await userManager.FindByEmailAsync(dto.Email);
        if (existing is not null)
            return Result<UserCreatedDto>.Failure(["Este correo ya está registrado"]);

        // 3. Crear Person con datos mínimos (el perfil completo se gestiona en HU-08)
        var person = new Person
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            MotherLastName = string.Empty,
            DateOfBirth = DateOnly.MinValue,
            Gender = Gender.Other,
            NationalId = string.Empty,
            NationalIdExpedition = string.Empty
        };
        var createdPerson = await personRepository.CreateAsync(person);

        // 4. Generar contraseña por defecto y crear el User mediante Identity (encripta automáticamente)
        var tempPassword = GenerateTemporaryPassword();
        var user = new User
        {
            UserName = dto.Email,
            Email = dto.Email,
            PersonId = createdPerson.Id
        };

        var identityResult = await userManager.CreateAsync(user, tempPassword);
        if (!identityResult.Succeeded)
            return Result<UserCreatedDto>.Failure(identityResult.Errors.Select(e => e.Description));

        // 5. Asignar rol al usuario
        await userManager.AddToRoleAsync(user, dto.Role);

        return Result<UserCreatedDto>.Success(new UserCreatedDto
        {
            Id = user.Id,
            FullName = $"{dto.FirstName} {dto.LastName}",
            Email = dto.Email,
            Role = dto.Role,
            TemporaryPassword = tempPassword
        });
    }

    /// <summary>
    /// Genera una contraseña temporal que cumple los requisitos de Identity:
    /// mayúscula, minúscula, dígito y carácter especial.
    /// </summary>
    private static string GenerateTemporaryPassword()
    {
        var suffix = Random.Shared.Next(100, 999);
        return $"Upds@{DateTime.UtcNow.Year}!{suffix}";
    }
}
