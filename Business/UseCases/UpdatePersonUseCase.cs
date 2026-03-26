using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Entities;
using Data.Repositories.Interfaces;
using FluentValidation;

namespace Business.UseCases;

public class UpdatePersonUseCase(
    IPersonRepository repository,
    IMapper mapper,
    IValidator<UpdatePersonDto> validator)
{
    public async Task<Result<PersonDto>> ExecuteAsync(int id, UpdatePersonDto dto)
    {
        var validation = await validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<PersonDto>.Failure(validation.Errors.Select(e => e.ErrorMessage));

        var person = await repository.GetByIdAsync(id);
        if (person == null)
            return Result<PersonDto>.Failure(["Persona no encontrada"]);

        if (dto.FirstName != null) person.FirstName = dto.FirstName;
        if (dto.LastName != null) person.LastName = dto.LastName;
        if (dto.MotherLastName != null) person.MotherLastName = dto.MotherLastName;
        if (dto.DateOfBirth.HasValue) person.DateOfBirth = dto.DateOfBirth.Value;
        if (dto.Gender.HasValue) person.Gender = dto.Gender.Value;
        if (dto.NationalId != null) person.NationalId = dto.NationalId;
        if (dto.NationalIdExpedition != null) person.NationalIdExpedition = dto.NationalIdExpedition;
        if (dto.PhoneNumber != null) person.PhoneNumber = dto.PhoneNumber;
        if (dto.Address != null) person.Address = dto.Address;

        await repository.UpdateAsync(person);

        return Result<PersonDto>.Success(mapper.Map<PersonDto>(person));
    }
}
