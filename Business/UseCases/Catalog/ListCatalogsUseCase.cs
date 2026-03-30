using AutoMapper;
using Business.DTOs.Responses;
using Business.Results;
using Data.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.UseCases.Catalog;

public class ListCatalogsUseCase(
    ICatalogRepository repository,
    IMapper mapper)
{
    public async Task<Result<IEnumerable<CatalogDto>>> ExecuteAsync()
    {
        var entities = await repository.GetAllAsync();
        return Result<IEnumerable<CatalogDto>>.Success(mapper.Map<IEnumerable<CatalogDto>>(entities));
    }
}