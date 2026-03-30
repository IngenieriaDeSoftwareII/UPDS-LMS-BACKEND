using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Entities;
using Data.Repositories.Implementations;
using Data.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Business.UseCases.Category;

public class CreateCategoryUseCase(
    ICategoryRepository repository,
    IMapper mapper)
{
    public async Task<Result<CategoryDto>> ExecuteAsync(CreateCategoryDto dto)
    {
        var entity = mapper.Map<Data.Entities.Category>(dto);
        var created = await repository.CreateAsync(entity);
        return Result<CategoryDto>.Success(mapper.Map<CategoryDto>(created));
    }
}