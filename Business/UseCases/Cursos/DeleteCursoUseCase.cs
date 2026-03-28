using Business.Results;
using Data.Repositories.Interfaces;

namespace Business.UseCases.Cursos;

public class DeleteCursoUseCase(ICursoRepository repository)
{
    public async Task<Result<bool>> ExecuteAsync(int id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity == null)
            return Result<bool>.Failure(["Curso no encontrado."]);

        await repository.DeleteAsync(id);
        return Result<bool>.Success(true);
    }
}
