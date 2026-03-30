using AutoMapper;
using Business.DTOs.Responses;
using Business.Results;
using Data.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Business.UseCases.Course;

public class GetCourseByIdUseCase(ICourseRepository repository, IMapper mapper)
{
    public async Task<Result<CourseDto>> ExecuteAsync(int id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity == null) return Result<CourseDto>.Failure(new[] { "Course not found" });
        return Result<CourseDto>.Success(mapper.Map<CourseDto>(entity));
    }
}