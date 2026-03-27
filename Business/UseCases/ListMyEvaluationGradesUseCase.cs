using Business.DTOs.Responses;
using Business.Results;
using Data.Repositories.Interfaces;

namespace Business.UseCases;

public class ListMyEvaluationGradesUseCase(
    IEvaluationRepository evaluationRepository,
    IUserRepository userRepository)
{
    public async Task<Result<IEnumerable<StudentGradeDto>>> ExecuteAsync(string currentUserId)
    {
        var user = await userRepository.FindByIdWithPersonAsync(currentUserId);
        if (user is null)
            return Result<IEnumerable<StudentGradeDto>>.Failure(["Usuario no encontrado."]);

        var attempts = await evaluationRepository.GetAttemptsByStudentAsync(user.PersonId);
        var grades = new List<StudentGradeDto>();

        foreach (var attempt in attempts)
        {
            var questions = attempt.Evaluaciones.Preguntas.ToList();
            var maxScore = questions.Sum(q => q.Puntos);

            grades.Add(new StudentGradeDto
            {
                EvaluacionId = attempt.EvaluacionId,
                TituloEvaluacion = attempt.Evaluaciones.Titulo,
                IntentoId = attempt.Id,
                NumeroIntento = attempt.NumeroIntento,
                PuntajeObtenido = attempt.PuntajeObtenido,
                PuntajeMaximo = maxScore,
                Aprobado = attempt.EsAprobado,
                FechaEnvio = attempt.CreatedAt
            });
        }

        return Result<IEnumerable<StudentGradeDto>>.Success(grades);
    }
}

