using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Entities;
using Data.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Business.UseCases.Teacher;

public class CreateTeacherUseCase(ITeacherRepository repository, IMapper mapper)
{
    public async Task<Result<TeacherDto>> ExecuteAsync(CreateTeacherDto dto)
    {
        var entity = mapper.Map<Data.Entities.Teacher>(dto);
        var created = await repository.CreateAsync(entity);
        return Result<TeacherDto>.Success(mapper.Map<TeacherDto>(created));
    }
}