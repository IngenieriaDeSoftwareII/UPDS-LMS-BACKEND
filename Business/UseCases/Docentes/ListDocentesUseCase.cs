using AutoMapper;
using Business.DTOs.Responses;
using Business.Results;
using Data.Repositories.Interfaces;

namespace Business.UseCases.Docentes;

public class ListDocentesUseCase(
    IDocenteRepository repository,
    IMapper mapper)
{
    public async Task<Result<IEnumerable<DocenteDto>>> ExecuteAsync()
    {
        var entities = await repository.GetAllAsync();
        return Result<IEnumerable<DocenteDto>>.Success(mapper.Map<IEnumerable<DocenteDto>>(entities));
    }
}
