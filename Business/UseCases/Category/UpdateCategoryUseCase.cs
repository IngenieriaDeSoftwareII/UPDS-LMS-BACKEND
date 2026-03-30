using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Repositories.Implementations;
using Data.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Business.UseCases.Category;

public class UpdateCategoryUseCase(
    ICategoryRepository repository,
    IMapper mapper)
{
    public async Task<Result<CategoryDto>> ExecuteAsync(UpdateCategoryDto dto)
    {
        var existingEntity = await repository.GetByIdAsync(dto.Id);
        if (existingEntity == null) return Result<CategoryDto>.Failure(["Category not found"]);

        mapper.Map(dto, existingEntity);
        await repository.UpdateAsync(existingEntity);

        return Result<CategoryDto>.Success(mapper.Map<CategoryDto>(existingEntity));
    }
}