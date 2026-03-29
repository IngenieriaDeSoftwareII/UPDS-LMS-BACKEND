using AutoMapper;
using Business.DTOs.Responses;
using Business.Results;
using Data.Repositories.Implementations;
using Data.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.UseCases.Category;

public class ListCategoriesUseCase(
    ICategoryRepository repository,
    IMapper mapper)
{
    public async Task<Result<IEnumerable<CategoryDto>>> ExecuteAsync()
    {
        var entities = await repository.GetAllAsync();
        return Result<IEnumerable<CategoryDto>>.Success(mapper.Map<IEnumerable<CategoryDto>>(entities));
    }
}