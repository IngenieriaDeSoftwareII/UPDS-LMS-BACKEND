using Business.DTOs.Responses;
using Business.Helpers;
using Business.Results;
using Data.Entities;
using Data.Enums;
using Data.Repositories.Interfaces;

namespace Business.UseCases.StudentProgress;

public class GetStudentCourseLearningUseCase(
    ICourseRepository courseRepository,
    IInscriptionRepository inscriptionRepository,
    ILessonProgressRepository lessonProgressRepository,
    ILessonRepository lessonRepository,
    IEvaluationRepository evaluationRepository)
{
    public async Task<Result<StudentCourseLearningDto>> ExecuteAsync(int personId, int cursoId)
    {
        var course = await courseRepository.GetByIdWithModulesLessonsAndTeacherAsync(cursoId);
        if (course is null)
            return Result<StudentCourseLearningDto>.Failure(["Curso no encontrado."]);

        var inscription = await inscriptionRepository.GetByUserAndCourseAsync(personId, cursoId);

        var progressByLesson = new Dictionary<int, bool>();
        if (inscription is not null)
        {
            var rows = await lessonProgressRepository.GetByUserAndCourseAsync(personId, cursoId);
            foreach (var p in rows)
            {
                if (p.Completado == true)
                    progressByLesson[p.LeccionId] = true;
            }
        }

        var totalLecciones = await lessonRepository.CountActiveLessonsByCourseAsync(cursoId);
        var completadas = progressByLesson.Count(kv => kv.Value);
        var pct = totalLecciones == 0
            ? 100
            : (int)Math.Round(100m * completadas / totalLecciones);

        var docenteNombre = BuildDocenteNombre(course.Docente);

        var modulos = course.Modulos
            .OrderBy(m => m.Orden)
            .Select(m => new StudentModuleLearningDto
            {
                Id = m.Id,
                Titulo = m.Titulo,
                Orden = m.Orden,
                Lecciones = m.Lecciones
                    .OrderBy(l => l.Orden ?? l.Id)
                    .Select(l => new StudentLessonLearningDto
                    {
                        Id = l.Id,
                        Titulo = l.Titulo,
                        Orden = l.Orden,
                        Completada = progressByLesson.ContainsKey(l.Id)
                    })
                    .ToList()
            })
            .ToList();

        var dto = new StudentCourseLearningDto
        {
            CursoId = course.Id,
            Titulo = course.Titulo,
            Descripcion = course.Descripcion,
            Nivel = course.Nivel,
            ImagenUrl = course.ImagenUrl,
            DuracionTotalMin = course.DuracionTotalMin,
            DocenteNombre = docenteNombre,
            CategoriaNombre = course.Categoria?.Nombre ?? string.Empty,
            Inscrito = inscription is not null,
            InscripcionId = inscription?.Id,
            EstadoInscripcion = inscription?.Estado.ToString(),
            LeccionesTotales = totalLecciones,
            LeccionesCompletadas = completadas,
            ProgresoPorcentaje = pct,
            Modulos = modulos
        };

        if (inscription is not null)
        {
            var (evaluation, bestAttempt) =
                await evaluationRepository.GetCourseEvaluationAndBestAttemptAsync(cursoId, personId);
            FillCertificateFields(dto, inscription, evaluation, bestAttempt);
        }

        return Result<StudentCourseLearningDto>.Success(dto);
    }

    private static string? BuildDocenteNombre(Data.Entities.Teacher? docente)
    {
        var p = docente?.Usuario?.Person;
        if (p is null) return null;
        return $"{p.FirstName} {p.LastName}".Trim();
    }

    private static void FillCertificateFields(
        StudentCourseLearningDto dto,
        Inscription inscription,
        Evaluation? evaluation,
        EvaluationAttempt? bestAttempt)
    {
        var completado = inscription.Estado == InscriptionEstate.Terminado;
        dto.CursoCompletado = completado;
        dto.TieneEvaluacionFinal = evaluation is not null;

        if (evaluation is null)
        {
            dto.PuedeDescargarCertificado = false;
            dto.AprobadoEvaluacionFinal = false;
            dto.MensajeCertificado = completado
                ? "Este curso no tiene evaluación final configurada; no se puede emitir el certificado."
                : null;
            return;
        }

        decimal? nota = null;
        if (bestAttempt is not null)
            nota = CertificateExamRules.ComputeNotaSobre100(evaluation, bestAttempt.PuntajeObtenido);

        dto.NotaEvaluacionSobre100 = nota;
        dto.AprobadoEvaluacionFinal = nota.HasValue && nota.Value >= CertificateExamRules.MinNotaSobre100;
        dto.PuedeDescargarCertificado = completado && dto.AprobadoEvaluacionFinal;

        if (!completado)
        {
            dto.MensajeCertificado = null;
            return;
        }

        if (bestAttempt is null)
        {
            dto.MensajeCertificado =
                "Debes rendir la evaluación final del curso (nota mínima 51/100) para descargar el certificado.";
            return;
        }

        if (!dto.AprobadoEvaluacionFinal)
        {
            dto.MensajeCertificado =
                $"No aprobaste la evaluación final. Tu nota es {nota:0.##}/100; se requiere mínimo 51.";
            return;
        }

        dto.MensajeCertificado = null;
    }
}
