using AutoMapper;
using Business.DTOs.Responses;
using Business.Results;
using Data.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Business.UseCases.Catalog;

public class GetCatalogByIdUseCase(
    ICatalogRepository repository,
    IMapper mapper)
{
    public async Task<Result<CatalogDto>> ExecuteAsync(int id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity == null) return Result<CatalogDto>.Failure(new[] { "Catalog not found" });
        
        return Result<CatalogDto>.Success(mapper.Map<CatalogDto>(entity));
    }
}