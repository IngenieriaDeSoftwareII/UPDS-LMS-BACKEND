using AutoMapper;
using Business.DTOs.Responses;
using Business.Results;
using Data.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.UseCases.Course;

public class ListCoursesByTeacherUseCase(ICourseRepository repository, IMapper mapper)
{
    public async Task<Result<IEnumerable<CourseDto>>> ExecuteAsync(int teacherId)
    {
        var entities = await repository.GetByTeacherIdAsync(teacherId);
        return Result<IEnumerable<CourseDto>>.Success(mapper.Map<IEnumerable<CourseDto>>(entities));
    }
}