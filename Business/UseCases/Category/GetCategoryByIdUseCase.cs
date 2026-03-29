using AutoMapper;
using Business.DTOs.Responses;
using Business.Results;
using Data.Repositories.Implementations;
using Data.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Business.UseCases.Category;

public class GetCategoryByIdUseCase(ICategoryRepository repository, IMapper mapper)
{
    public async Task<Result<CategoryDto>> ExecuteAsync(int id)
    {
        var category = await repository.GetByIdAsync(id);
        if (category == null) return Result<CategoryDto>.Failure(["Category not found"]);

        return Result<CategoryDto>.Success(mapper.Map<CategoryDto>(category));
    }
}