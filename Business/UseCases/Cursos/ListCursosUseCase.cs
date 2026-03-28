using AutoMapper;
using Business.DTOs.Responses;
using Business.Results;
using Data.Repositories.Interfaces;

namespace Business.UseCases.Cursos;

public class ListCursosUseCase(
    ICursoRepository repository,
    IMapper mapper)
{
    public async Task<Result<IEnumerable<CursoDto>>> ExecuteAsync()
    {
        var cursos = await repository.GetAllAsync();
        return Result<IEnumerable<CursoDto>>.Success(mapper.Map<IEnumerable<CursoDto>>(cursos));
    }
}
