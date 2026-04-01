using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Entities;
using Data.Repositories.Interfaces;
using FluentValidation;

namespace Business.UseCases;

public class CreateEvaluationUseCase(
    IEvaluationRepository evaluationRepository,
    ICourseRepository courseRepository,
    IUserRepository userRepository,
    ITeacherRepository teacherRepository,
    IValidator<CreateEvaluationDto> validator,
    IMapper mapper)
{
    public async Task<Result<EvaluationDto>> ExecuteAsync(string currentUserId, CreateEvaluationDto dto)
    {
        var validation = await validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<EvaluationDto>.Failure(validation.Errors.Select(e => e.ErrorMessage));

        var user = await userRepository.FindByIdWithPersonAsync(currentUserId);
        if (user is null)
            return Result<EvaluationDto>.Failure(["Usuario no encontrado."]);

        var teacher = await teacherRepository.GetByUserIdAsync(currentUserId);
        if (teacher is null)
            return Result<EvaluationDto>.Failure(["Docente no encontrado."]);

        var course = await courseRepository.GetByIdAsync(dto.CursoId);
        if (course is null)
            return Result<EvaluationDto>.Failure(["El curso no existe."]);

        if (course.DocenteId != teacher.Id)
            return Result<EvaluationDto>.Failure(["No puedes crear evaluaciones para cursos que no son tuyos."]);

        var evaluation = mapper.Map<Evaluation>(dto);
        var created = await evaluationRepository.CreateAsync(evaluation);
        return Result<EvaluationDto>.Success(mapper.Map<EvaluationDto>(created));
    }
}

