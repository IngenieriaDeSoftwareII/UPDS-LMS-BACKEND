using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Business.UseCases.Course;

public class UpdateCourseUseCase(ICourseRepository repository, IMapper mapper)
{
    public async Task<Result<CourseDto>> ExecuteAsync(UpdateCourseDto dto)
    {
        var entity = await repository.GetByIdAsync(dto.Id);
        if (entity == null) return Result<CourseDto>.Failure(new[] { "Course not found" });
        mapper.Map(dto, entity);
        await repository.UpdateAsync(entity);
        return Result<CourseDto>.Success(mapper.Map<CourseDto>(entity));
    }
}