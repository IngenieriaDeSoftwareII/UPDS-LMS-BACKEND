using Business.DTOs.Responses;
using Business.Results;
using Data.Repositories.Interfaces;

namespace Business.UseCases.StudentProgress;

public class GetStudentProgressDashboardUseCase(
    IInscriptionRepository inscriptionRepository,
    ILessonRepository lessonRepository,
    ILessonProgressRepository lessonProgressRepository)
{
    public async Task<Result<IReadOnlyList<StudentDashboardProgressDto>>> ExecuteAsync(int personId)
    {
        var inscriptions = await inscriptionRepository.GetByUserAsync(personId);
        var list = new List<StudentDashboardProgressDto>();

        foreach (var ins in inscriptions)
        {
            var curso = ins.Cursos;
            var total = await lessonRepository.CountActiveLessonsByCourseAsync(curso.Id);
            var progressRows = await lessonProgressRepository.GetByUserAndCourseAsync(personId, curso.Id);
            var completadas = progressRows.Count(p => p.Completado == true);
            var pct = total == 0
                ? 100
                : (int)Math.Round(100m * completadas / total);

            list.Add(new StudentDashboardProgressDto
            {
                CursoId = curso.Id,
                Titulo = curso.Titulo,
                ImagenUrl = curso.ImagenUrl,
                EstadoInscripcion = ins.Estado.ToString(),
                FechaCompletado = ins.FechaCompletado,
                ProgresoPorcentaje = pct,
                LeccionesCompletadas = completadas,
                LeccionesTotales = total
            });
        }

        return Result<IReadOnlyList<StudentDashboardProgressDto>>.Success(list);
    }
}
