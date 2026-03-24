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
        var validation = await validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<UserCreatedDto>.Failure(validation.Errors.Select(e => e.ErrorMessage));

        var existing = await userManager.FindByEmailAsync(dto.Email);
        if (existing is not null)
            return Result<UserCreatedDto>.Failure(["Este correo ya está registrado"]);

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

    private static string GenerateTemporaryPassword()
    {
        var suffix = Random.Shared.Next(100, 999);
        return $"Upds@{DateTime.UtcNow.Year}!{suffix}";
    }
}
