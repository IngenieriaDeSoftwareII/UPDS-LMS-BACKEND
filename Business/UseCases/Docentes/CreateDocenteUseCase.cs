using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Entities;
using Data.Repositories.Interfaces;

namespace Business.UseCases.Docentes;

public class CreateDocenteUseCase(
    IDocenteRepository repository,
    IMapper mapper)
{
    public async Task<Result<DocenteDto>> ExecuteAsync(CreateDocenteDto dto)
    {
        var docente = mapper.Map<Docente>(dto);
        var created = await repository.CreateAsync(docente);
        return Result<DocenteDto>.Success(mapper.Map<DocenteDto>(created));
    }
}
