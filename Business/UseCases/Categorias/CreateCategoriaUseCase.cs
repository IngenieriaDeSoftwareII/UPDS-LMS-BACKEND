using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Entities;
using Data.Repositories.Interfaces;
using FluentValidation;

namespace Business.UseCases.Categorias;

public class CreateCategoriaUseCase(
    ICategoriaRepository repository,
    IMapper mapper,
    IValidator<CreateCategoriaDto> validator)
{
    public async Task<Result<CategoriaDto>> ExecuteAsync(CreateCategoriaDto dto)
    {
        var validation = await validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<CategoriaDto>.Failure(validation.Errors.Select(e => e.ErrorMessage));

        var categoria = mapper.Map<Categoria>(dto);
        var created = await repository.CreateAsync(categoria);

        return Result<CategoriaDto>.Success(mapper.Map<CategoriaDto>(created));
    }
}
