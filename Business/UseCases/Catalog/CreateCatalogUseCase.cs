using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Entities;
using Data.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Business.UseCases.Catalog;

public class CreateCatalogUseCase(
    ICatalogRepository repository,
    IMapper mapper)
{
    public async Task<Result<CatalogDto>> ExecuteAsync(CreateCatalogDto dto)
    {
        var entity = mapper.Map<Data.Entities.Catalog>(dto);
        var created = await repository.CreateAsync(entity);
        return Result<CatalogDto>.Success(mapper.Map<CatalogDto>(created));
    }
}