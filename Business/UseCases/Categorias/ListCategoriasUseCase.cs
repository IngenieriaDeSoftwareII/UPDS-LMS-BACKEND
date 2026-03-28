using AutoMapper;
using Business.DTOs.Responses;
using Business.Results;
using Data.Repositories.Interfaces;

namespace Business.UseCases.Categorias;

public class ListCategoriasUseCase(
    ICategoriaRepository repository,
    IMapper mapper)
{
    public async Task<Result<IEnumerable<CategoriaDto>>> ExecuteAsync()
    {
        var categorias = await repository.GetAllAsync();
        return Result<IEnumerable<CategoriaDto>>.Success(mapper.Map<IEnumerable<CategoriaDto>>(categorias));
    }
}
