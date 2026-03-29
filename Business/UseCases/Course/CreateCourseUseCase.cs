using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Entities;
using Data.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Business.UseCases.Course;

public class CreateCourseUseCase(ICourseRepository repository, IMapper mapper)
{
    public async Task<Result<CourseDto>> ExecuteAsync(CreateCourseDto dto)
    {
        var entity = mapper.Map<Data.Entities.Course>(dto);
        var created = await repository.CreateAsync(entity);
        return Result<CourseDto>.Success(mapper.Map<CourseDto>(created));
    }
}