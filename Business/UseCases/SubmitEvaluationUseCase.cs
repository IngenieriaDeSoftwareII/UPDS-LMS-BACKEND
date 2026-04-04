using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Entities;
using Data.Enums;
using Data.Repositories.Interfaces;
using FluentValidation;

namespace Business.UseCases;

public class SubmitEvaluationUseCase(
    IEvaluationRepository evaluationRepository,
    IInscriptionRepository inscriptionRepository,
    IUserRepository userRepository,
    IValidator<SubmitEvaluationDto> validator)
{
    public async Task<Result<EvaluationSubmissionResultDto>> ExecuteAsync(string currentUserId, SubmitEvaluationDto dto)
    {
        var validation = await validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<EvaluationSubmissionResultDto>.Failure(validation.Errors.Select(e => e.ErrorMessage));

        var user = await userRepository.FindByIdWithPersonAsync(currentUserId);
        if (user is null)
            return Result<EvaluationSubmissionResultDto>.Failure(["Usuario no encontrado."]);

        var evaluation = await evaluationRepository.GetByIdWithQuestionsAsync(dto.EvaluacionId);
        if (evaluation is null)
            return Result<EvaluationSubmissionResultDto>.Failure(["La evaluación no existe."]);

        var inscription = await inscriptionRepository.GetByUserAndCourseAsync(user.PersonId, evaluation.CursoId);
        if (inscription is null || inscription.Estado == InscriptionEstate.Cancelado)
            return Result<EvaluationSubmissionResultDto>.Failure(["No estás inscrito en este curso."]);

        var courseCompleted =
            inscription.Estado == InscriptionEstate.Terminado ||
            inscription.FechaCompletado.HasValue;

        if (!courseCompleted)
            return Result<EvaluationSubmissionResultDto>.Failure(
                ["Solo puedes responder la evaluación cuando hayas completado el curso."]);

        var attemptsCount = await evaluationRepository.CountAttemptsAsync(evaluation.Id, user.PersonId);
        if (attemptsCount >= evaluation.IntentosPermitidos)
            return Result<EvaluationSubmissionResultDto>.Failure(["Ya alcanzaste el máximo de intentos permitidos."]);

        var questionsById = evaluation.Preguntas.ToDictionary(q => q.Id);
        var sentQuestionIds = dto.Respuestas.Select(r => r.PreguntaId).Distinct().ToHashSet();

        if (questionsById.Keys.Any(qid => !sentQuestionIds.Contains(qid)))
            return Result<EvaluationSubmissionResultDto>.Failure(["Debes responder todas las preguntas de la evaluación."]);

        var attempt = new EvaluationAttempt
        {
            EvaluacionId = evaluation.Id,
            UsuarioId = user.PersonId,
            NumeroIntento = attemptsCount + 1
        };

        var answers = new List<EvaluationAnswer>();
        var score = 0m;
        var maxScore = 0m;

        foreach (var question in evaluation.Preguntas)
        {
            maxScore += question.Puntos;

            var answerDto = dto.Respuestas.First(r => r.PreguntaId == question.Id);
            var questionOptions = question.OpcionesRespuesta.ToList();
            var correctOption = questionOptions.FirstOrDefault(o => o.EsCorrecta);

            var isCorrect = false;
            if (answerDto.OpcionRespuestaId.HasValue && correctOption is not null)
                isCorrect = answerDto.OpcionRespuestaId.Value == correctOption.Id;

            if (isCorrect)
                score += question.Puntos;

            answers.Add(new EvaluationAnswer
            {
                PreguntaId = question.Id,
                OpcionRespuestaId = answerDto.OpcionRespuestaId,
                RespuestaTexto = answerDto.RespuestaTexto
            });
        }

        var approvalMin = evaluation.PuntajeMinimoAprobacion ?? (maxScore * 0.51m);
        attempt.PuntajeObtenido = score;
        attempt.EsAprobado = score >= approvalMin;

        var createdAttempt = await evaluationRepository.CreateAttemptAsync(attempt, answers);

        return Result<EvaluationSubmissionResultDto>.Success(new EvaluationSubmissionResultDto
        {
            EvaluacionId = evaluation.Id,
            IntentoId = createdAttempt.Id,
            NumeroIntento = createdAttempt.NumeroIntento,
            PuntajeObtenido = createdAttempt.PuntajeObtenido,
            PuntajeMaximo = maxScore,
            Aprobado = createdAttempt.EsAprobado
        });
    }
}

