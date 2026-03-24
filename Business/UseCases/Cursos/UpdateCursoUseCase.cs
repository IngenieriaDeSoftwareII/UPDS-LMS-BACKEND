using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Entities;
using Data.Repositories.Interfaces;

namespace Business.UseCases.Cursos;

public class UpdateCursoUseCase(
    ICursoRepository repository,
    IMapper mapper)
{
    public async Task<Result<CursoDto>> ExecuteAsync(UpdateCursoDto dto)
    {
        var entity = await repository.GetByIdAsync(dto.Id);
        if (entity == null)
            return Result<CursoDto>.Failure(["Curso no encontrado."]);

        mapper.Map(dto, entity);
        await repository.UpdateAsync(entity);

        return Result<CursoDto>.Success(mapper.Map<CursoDto>(entity));
    }
}
