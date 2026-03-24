using AutoMapper;
using Business.DTOs.Responses;
using Business.Results;
using Data.Repositories.Interfaces;

namespace Business.UseCases.Cursos;

public class GetCursoByIdUseCase(ICursoRepository repository, IMapper mapper)
{
    public async Task<Result<CursoDto>> ExecuteAsync(int id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity == null)
            return Result<CursoDto>.Failure(["Curso no encontrado."]);

        return Result<CursoDto>.Success(mapper.Map<CursoDto>(entity));
    }
}
