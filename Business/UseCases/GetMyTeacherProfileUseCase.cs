using Business.DTOs.Responses;
using Business.Results;
using Data.Repositories.Interfaces;

namespace Business.UseCases;

public class GetMyTeacherProfileUseCase(ITeacherRepository teacherRepository)
{
    public async Task<Result<TeacherProfileDto>> ExecuteAsync(string userId)
    {
        var teacher = await teacherRepository.GetByUserIdAsync(userId);
        if (teacher is null)
            return Result<TeacherProfileDto>.Failure(["Docente no encontrado"]);

        return Result<TeacherProfileDto>.Success(new TeacherProfileDto
        {
            TeacherId = teacher.Id,
            FirstName = teacher.Usuario.Person.FirstName,
            LastName = teacher.Usuario.Person.LastName,
            Especialidad = teacher.Especialidad,
            Biografia = teacher.Biografia
        });
    }
}