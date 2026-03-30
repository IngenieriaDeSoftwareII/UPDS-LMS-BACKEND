using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Entities;
using Data.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Business.UseCases.Catalog;

public class UpdateCatalogUseCase(
    ICatalogRepository repository,
    IMapper mapper)
{
    public async Task<Result<CatalogDto>> ExecuteAsync(UpdateCatalogDto dto)
    {
        var existingEntity = await repository.GetByIdAsync(dto.Id);
        if (existingEntity == null) return Result<CatalogDto>.Failure(new[] { "Catalog not found" });

        mapper.Map(dto, existingEntity);
        await repository.UpdateAsync(existingEntity);

        return Result<CatalogDto>.Success(mapper.Map<CatalogDto>(existingEntity));
    }
}