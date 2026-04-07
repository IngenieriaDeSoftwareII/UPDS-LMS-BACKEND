using Business.Results;
using Data.Repositories.Interfaces;

namespace Business.UseCases.ImageContent;
public class DeleteImageContentUseCase(IImageContentRepository repository)
{
    public async Task<Result<bool>> ExecuteAsync(int contentId)
    {
        var existing = await repository.GetByContentIdAsync(contentId);
        if (existing is null)
            return Result<bool>.Failure(["ImageContent no encontrado"]);

        await repository.DeleteAsync(contentId);

        return Result<bool>.Success(true);
    }
}