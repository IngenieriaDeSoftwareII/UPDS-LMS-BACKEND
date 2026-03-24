using Business.Results;
using Data.Repositories.Interfaces;

namespace Business.UseCases;

public class DeleteVideoContentUseCase(IVideoContentRepository repository)
{
    public async Task<Result<bool>> ExecuteAsync(int contentId)
    {
        var existing = await repository.GetByContentIdAsync(contentId);
        if (existing is null)
            return Result<bool>.Failure(["VideoContent no encontrado"]);

        await repository.DeleteAsync(contentId);

        return Result<bool>.Success(true);
    }
}