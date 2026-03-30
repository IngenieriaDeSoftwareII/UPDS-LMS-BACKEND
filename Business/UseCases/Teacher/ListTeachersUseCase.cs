using AutoMapper;
using Business.DTOs.Responses;
using Business.Results;
using Data.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.UseCases.Teacher;

public class ListTeachersUseCase(ITeacherRepository repository, IMapper mapper)
{
    public async Task<Result<IEnumerable<TeacherDto>>> ExecuteAsync()
    {
        var entities = await repository.GetAllAsync();
        return Result<IEnumerable<TeacherDto>>.Success(mapper.Map<IEnumerable<TeacherDto>>(entities));
    }
}