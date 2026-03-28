using Business.DTOs.Responses;
using Business.Results;
using Data.Enums;
using Data.Repositories.Interfaces;

namespace Business.UseCases;

public class GetEvaluationToTakeUseCase(
    IEvaluationRepository evaluationRepository,
    IInscriptionRepository inscriptionRepository,
    IUserRepository userRepository)
{
    public async Task<Result<EvaluationToTakeDto>> ExecuteAsync(string currentUserId, int cursoId)
    {
        var user = await userRepository.FindByIdWithPersonAsync(currentUserId);
        if (user is null)
            return Result<EvaluationToTakeDto>.Failure(["Usuario no encontrado."]);

        var evaluation = await evaluationRepository.GetByCourseIdWithQuestionsAsync(cursoId);
        if (evaluation is null)
            return Result<EvaluationToTakeDto>.Failure(["No existe una evaluación para este curso."]);

        var inscription = await inscriptionRepository.GetByUserAndCourseAsync(user.PersonId, evaluation.CursoId);
        if (inscription is null || inscription.Estado == InscriptionEstate.Cancelado)
            return Result<EvaluationToTakeDto>.Failure(["No estás inscrito en este curso."]);

        var courseCompleted =
            inscription.Estado == InscriptionEstate.Terminado ||
            inscription.FechaCompletado.HasValue;

        if (!courseCompleted)
            return Result<EvaluationToTakeDto>.Failure(["Solo puedes responder la evaluación cuando hayas completado el curso."]);

        var dto = new EvaluationToTakeDto
        {
            Id = evaluation.Id,
            CursoId = evaluation.CursoId,
            Titulo = evaluation.Titulo,
            Descripcion = evaluation.Descripcion,
            Tipo = evaluation.Tipo,
            PuntajeMaximo = evaluation.PuntajeMaximo,
            PuntajeMinimoAprobacion = evaluation.PuntajeMinimoAprobacion,
            IntentosPermitidos = evaluation.IntentosPermitidos,
            TiempoLimiteMax = evaluation.TiempoLimiteMax,
            Preguntas = evaluation.Preguntas
                .OrderBy(p => p.Orden)
                .Select(q => new EvaluationQuestionToTakeDto
                {
                    Id = q.Id,
                    EvaluacionId = q.EvaluacionId,
                    Enunciado = q.Enunciado,
                    Tipo = q.Tipo,
                    Puntos = q.Puntos,
                    Orden = q.Orden,
                    Opciones = q.OpcionesRespuesta
                        .OrderBy(o => o.Orden)
                        .Select(o => new EvaluationAnswerOptionToTakeDto
                        {
                            Id = o.Id,
                            Texto = o.Texto,
                            Orden = o.Orden
                        })
                        .ToList()
                })
                .ToList()
        };

        return Result<EvaluationToTakeDto>.Success(dto);
    }
}

