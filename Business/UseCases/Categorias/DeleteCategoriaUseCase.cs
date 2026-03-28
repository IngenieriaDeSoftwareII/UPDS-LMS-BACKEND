using Business.Results;
using Data.Repositories.Interfaces;

namespace Business.UseCases.Categorias;

public class DeleteCategoriaUseCase(ICategoriaRepository repository)
{
    public async Task<Result<bool>> ExecuteAsync(int id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity == null)
            return Result<bool>.Failure(["Categoría no encontrada."]);

        await repository.DeleteAsync(id);
        return Result<bool>.Success(true);
    }
}
