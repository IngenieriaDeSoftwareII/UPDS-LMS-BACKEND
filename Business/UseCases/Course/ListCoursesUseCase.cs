using AutoMapper;
using Business.DTOs.Responses;
using Business.Results;
using Data.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.UseCases.Course;

public class ListCoursesUseCase(ICourseRepository repository, IMapper mapper)
{
    public async Task<Result<IEnumerable<CourseDto>>> ExecuteAsync()
    {
        var entities = await repository.GetAllAsync();
        return Result<IEnumerable<CourseDto>>.Success(mapper.Map<IEnumerable<CourseDto>>(entities));
    }
}