using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Entities;
using Data.Repositories.Interfaces;
using FluentValidation;

namespace Business.UseCases;

public class CreatePersonUseCase(
    IPersonRepository repository,
    IMapper mapper,
    IValidator<CreatePersonDto> validator)
{
    public async Task<Result<PersonDto>> ExecuteAsync(CreatePersonDto dto)
    {
        var validation = await validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<PersonDto>.Failure(validation.Errors.Select(e => e.ErrorMessage));

        var person = mapper.Map<Person>(dto);
        var created = await repository.CreateAsync(person);

        return Result<PersonDto>.Success(mapper.Map<PersonDto>(created));
    }
}
