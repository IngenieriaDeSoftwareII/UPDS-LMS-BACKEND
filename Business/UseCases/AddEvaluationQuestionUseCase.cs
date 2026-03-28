using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Repositories.Interfaces;
using FluentValidation;

namespace Business.UseCases;

public class AddEvaluationQuestionUseCase(
    IEvaluationRepository evaluationRepository,
    ICourseRepository courseRepository,
    IUserRepository userRepository,
    IValidator<AddEvaluationQuestionDto> validator,
    IMapper mapper)
{
    public async Task<Result<EvaluationQuestionDto>> ExecuteAsync(string currentUserId, AddEvaluationQuestionDto dto)
    {
        var validation = await validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<EvaluationQuestionDto>.Failure(validation.Errors.Select(e => e.ErrorMessage));

        var user = await userRepository.FindByIdWithPersonAsync(currentUserId);
        if (user is null)
            return Result<EvaluationQuestionDto>.Failure(["Usuario no encontrado."]);

        var evaluation = await evaluationRepository.GetByIdAsync(dto.EvaluacionId);
        if (evaluation is null)
            return Result<EvaluationQuestionDto>.Failure(["La evaluación no existe."]);

        if (evaluation.CursoId <= 0)
            return Result<EvaluationQuestionDto>.Failure(["La evaluación no tiene curso asociado."]);

        var course = await courseRepository.GetByIdAsync(evaluation.CursoId);
        if (course is null)
            return Result<EvaluationQuestionDto>.Failure(["El curso asociado no existe."]);

        if (course.DocenteId != user.PersonId)
            return Result<EvaluationQuestionDto>.Failure(["No puedes configurar preguntas en cursos que no son tuyos."]);

        var question = mapper.Map<Data.Entities.Question>(dto);
        var options = dto.Opciones.Select(mapper.Map<Data.Entities.AnswerOption>).ToList();

        var createdQuestion = await evaluationRepository.AddQuestionAsync(question, options);
        createdQuestion.OpcionesRespuesta = options;

        return Result<EvaluationQuestionDto>.Success(mapper.Map<EvaluationQuestionDto>(createdQuestion));
    }
}

