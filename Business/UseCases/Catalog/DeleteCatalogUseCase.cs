using Business.Results;
using Data.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Business.UseCases.Catalog;

public class DeleteCatalogUseCase(ICatalogRepository repository)
{
    public async Task<Result<bool>> ExecuteAsync(int id)
    {
        var existingEntity = await repository.GetByIdAsync(id);
        if (existingEntity == null) return Result<bool>.Failure(new[] { "Catalog not found" });

        await repository.DeleteAsync(id);
        return Result<bool>.Success(true);
    }
}