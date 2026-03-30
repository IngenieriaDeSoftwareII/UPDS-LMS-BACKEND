using Business.Results;
using Data.Repositories.Implementations;
using Data.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Business.UseCases.Category;

public class DeleteCategoryUseCase(ICategoryRepository repository)
{
    public async Task<Result<bool>> ExecuteAsync(int id)
    {
        var existingEntity = await repository.GetByIdAsync(id);
        if (existingEntity == null) return Result<bool>.Failure(["Category not found"]);

        await repository.DeleteAsync(id);
        return Result<bool>.Success(true);
    }
}