using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Repositories.Interfaces;
using FluentValidation;

namespace Business.UseCases;

public class UpdateEvaluationUseCase(
    IEvaluationRepository evaluationRepository,
    ICourseRepository courseRepository,
    IUserRepository userRepository,
    ITeacherRepository teacherRepository,
    IValidator<CreateEvaluationDto> validator,
    IMapper mapper)
{
    public async Task<Result<EvaluationDto>> ExecuteAsync(string currentUserId, int evaluationId, CreateEvaluationDto dto)
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

        var evaluation = await evaluationRepository.GetByIdAsync(evaluationId);
        if (evaluation is null)
            return Result<EvaluationDto>.Failure(["La evaluación no existe."]);

        var course = await courseRepository.GetByIdAsync(dto.CursoId);
        if (course is null)
            return Result<EvaluationDto>.Failure(["El curso no existe."]);

        if (course.DocenteId != teacher.Id)
            return Result<EvaluationDto>.Failure(["No puedes actualizar evaluaciones de cursos que no son tuyos."]);

        if (evaluation.CursoId != dto.CursoId && evaluation.Cursos?.DocenteId != teacher.Id)
            return Result<EvaluationDto>.Failure(["No puedes actualizar evaluaciones que no son tuyas."]);

        evaluation.Titulo = dto.Titulo;
        evaluation.Descripcion = dto.Descripcion;
        evaluation.Tipo = dto.Tipo;
        evaluation.PuntajeMaximo = dto.PuntajeMaximo;
        evaluation.PuntajeMinimoAprobacion = dto.PuntajeMinimoAprobacion;
        evaluation.IntentosPermitidos = dto.IntentosPermitidos;
        evaluation.TiempoLimiteMax = dto.TiempoLimiteMax;

        var updated = await evaluationRepository.UpdateAsync(evaluation);
        return Result<EvaluationDto>.Success(mapper.Map<EvaluationDto>(updated));
    }
}
