using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Entities;
using Data.Repositories.Interfaces;

namespace Business.UseCases.Categorias;

public class UpdateCategoriaUseCase(
    ICategoriaRepository repository,
    IMapper mapper)
{
    public async Task<Result<CategoriaDto>> ExecuteAsync(UpdateCategoriaDto dto)
    {
        var entity = await repository.GetByIdAsync(dto.Id);
        if (entity == null)
            return Result<CategoriaDto>.Failure(["Categoría no encontrada."]);

        mapper.Map(dto, entity);
        await repository.UpdateAsync(entity);

        return Result<CategoriaDto>.Success(mapper.Map<CategoriaDto>(entity));
    }
}
