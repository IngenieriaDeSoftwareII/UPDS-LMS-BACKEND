using Business.Results;
using Data.Repositories.Interfaces;
using Data.Entities;

namespace Business.UseCases.Homework;

public class DeleteHomeworkUseCase(IHomeworkRepository repository)
{
    public async Task<Result<bool>> ExecuteAsync(int id)
    {
        var homework = await repository.GetByIdAsync(id);
        if (homework == null)
            return Result<bool>.Failure(["La tarea no existe."]);

        await repository.DeleteAsync(id);
        return Result<bool>.Success(true);
    }
}