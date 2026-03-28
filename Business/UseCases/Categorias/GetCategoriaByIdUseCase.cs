using AutoMapper;
using Business.DTOs.Responses;
using Business.Results;
using Data.Repositories.Interfaces;

namespace Business.UseCases.Categorias;

public class GetCategoriaByIdUseCase(ICategoriaRepository repository, IMapper mapper)
{
    public async Task<Result<CategoriaDto>> ExecuteAsync(int id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity == null)
            return Result<CategoriaDto>.Failure(["Categoría no encontrada."]);

        return Result<CategoriaDto>.Success(mapper.Map<CategoriaDto>(entity));
    }
}
