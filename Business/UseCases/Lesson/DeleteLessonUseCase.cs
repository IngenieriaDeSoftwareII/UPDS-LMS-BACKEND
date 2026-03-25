using Business.Results;
using Data.Repositories.Interfaces;

namespace Business.UseCases.Lesson;

public class DeleteLessonUseCase(ILessonRepository repository)
{
    public async Task<Result<bool>> ExecuteAsync(int id)
    {
        var existing = await repository.GetByIdAsync(id);
        if (existing is null)
            return Result<bool>.Failure(["Lesson no encontrada"]);

        await repository.DeleteAsync(id);

        return Result<bool>.Success(true);
    }
}