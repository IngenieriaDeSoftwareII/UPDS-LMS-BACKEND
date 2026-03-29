using Business.Results;
using Data.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Business.UseCases.Course;

public class DeleteCourseUseCase(ICourseRepository repository)
{
    public async Task<Result<bool>> ExecuteAsync(int id)
    {
        var existingEntity = await repository.GetByIdAsync(id);
        if (existingEntity == null) return Result<bool>.Failure(new[] { "Course not found" });

        await repository.DeleteAsync(id);
        return Result<bool>.Success(true);
    }
}