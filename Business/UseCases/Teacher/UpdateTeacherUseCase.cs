using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Business.UseCases.Teacher;

public class UpdateTeacherUseCase(
    ITeacherRepository repository,
    IMapper mapper)
{
    public async Task<Result<TeacherDto>> ExecuteAsync(UpdateTeacherDto dto)
    {
        var existingEntity = await repository.GetByIdAsync(dto.Id);
        if (existingEntity == null) return Result<TeacherDto>.Failure(new[] { "Teacher not found" });

        mapper.Map(dto, existingEntity);
        await repository.UpdateAsync(existingEntity);

        return Result<TeacherDto>.Success(mapper.Map<TeacherDto>(existingEntity));
    }
}