using Business.DTOs.Responses;
using Business.Results;
using Data.Repositories.Interfaces;

namespace Business.UseCases;

public class ListEvaluationGradesForTeacherUseCase(
    IEvaluationRepository evaluationRepository,
    ICourseRepository courseRepository,
    IUserRepository userRepository,
    ITeacherRepository teacherRepository,
    IPersonRepository personRepository)
{
    public async Task<Result<IEnumerable<TeacherEvaluationGradeDto>>> ExecuteAsync(string currentUserId, int cursoId)
    {
        var user = await userRepository.FindByIdWithPersonAsync(currentUserId);
        if (user is null)
            return Result<IEnumerable<TeacherEvaluationGradeDto>>.Failure(["Usuario no encontrado."]);

        var teacher = await teacherRepository.GetByUserIdAsync(currentUserId);
        if (teacher is null)
            return Result<IEnumerable<TeacherEvaluationGradeDto>>.Failure(["Docente no encontrado."]);

        var course = await courseRepository.GetByIdAsync(cursoId);
        if (course is null)
            return Result<IEnumerable<TeacherEvaluationGradeDto>>.Failure(["El curso asociado no existe."]);

        if (course.DocenteId != teacher.Id)
            return Result<IEnumerable<TeacherEvaluationGradeDto>>.Failure(["No puedes ver notas de cursos que no son tuyos."]);

        var evaluation = await evaluationRepository.GetByCourseIdWithQuestionsAsync(cursoId);
        if (evaluation is null)
            return Result<IEnumerable<TeacherEvaluationGradeDto>>.Failure(["No existe una evaluación para este curso."]);

        var attempts = await evaluationRepository.GetAttemptsByEvaluationAsync(evaluation.Id);
        var grades = new List<TeacherEvaluationGradeDto>();

        foreach (var attempt in attempts)
        {
            var questions = attempt.Evaluaciones.Preguntas.ToList();
            var maxScore = questions.Sum(q => q.Puntos);
            var person = await personRepository.GetByIdAsync(attempt.UsuarioId);
            var fullName = person is null
                ? $"Estudiante {attempt.UsuarioId}"
                : $"{person.FirstName} {person.LastName} {person.MotherLastName}".Trim();

            grades.Add(new TeacherEvaluationGradeDto
            {
                EvaluacionId = attempt.EvaluacionId,
                TituloEvaluacion = attempt.Evaluaciones.Titulo,
                EstudianteId = attempt.UsuarioId,
                EstudianteNombreCompleto = fullName,
                IntentoId = attempt.Id,
                NumeroIntento = attempt.NumeroIntento,
                PuntajeObtenido = attempt.PuntajeObtenido,
                PuntajeMaximo = maxScore,
                Aprobado = attempt.EsAprobado,
                FechaEnvio = attempt.CreatedAt
            });
        }

        return Result<IEnumerable<TeacherEvaluationGradeDto>>.Success(grades);
    }
}

