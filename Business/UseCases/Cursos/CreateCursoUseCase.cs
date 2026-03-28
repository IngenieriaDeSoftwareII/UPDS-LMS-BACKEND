using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Entities;
using Data.Repositories.Interfaces;
using FluentValidation;

namespace Business.UseCases.Cursos;

public class CreateCursoUseCase(
    ICursoRepository repository,
    IMapper mapper,
    IValidator<CreateCursoDto> validator)
{
    public async Task<Result<CursoDto>> ExecuteAsync(CreateCursoDto dto)
    {
        var validation = await validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<CursoDto>.Failure(validation.Errors.Select(e => e.ErrorMessage));

        var curso = mapper.Map<Curso>(dto);
        var created = await repository.CreateAsync(curso);

        return Result<CursoDto>.Success(mapper.Map<CursoDto>(created));
    }
}
