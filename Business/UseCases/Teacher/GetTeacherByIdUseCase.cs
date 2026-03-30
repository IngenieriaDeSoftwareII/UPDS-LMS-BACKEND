using AutoMapper;
using Business.DTOs.Responses;
using Business.Results;
using Data.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Business.UseCases.Teacher;

public class GetTeacherByIdUseCase(
    ITeacherRepository repository,
    IMapper mapper)
{
    public async Task<Result<TeacherDto>> ExecuteAsync(int id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity == null) return Result<TeacherDto>.Failure(new[] { "Teacher not found" });
        
        return Result<TeacherDto>.Success(mapper.Map<TeacherDto>(entity));
    }
}