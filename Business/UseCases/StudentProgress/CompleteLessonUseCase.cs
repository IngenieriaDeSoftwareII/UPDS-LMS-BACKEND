using Business.DTOs.Responses;
using Business.Results;
using Data.Entities;
using Data.Enums;
using Data.Repositories.Interfaces;

namespace Business.UseCases.StudentProgress;

public class CompleteLessonUseCase(
    ILessonRepository lessonRepository,
    IInscriptionRepository inscriptionRepository,
    ILessonProgressRepository lessonProgressRepository)
{
    public async Task<Result<CompleteLessonResponseDto>> ExecuteAsync(int personId, int leccionId)
    {
        var lesson = await lessonRepository.GetByIdWithModuleAndCourseAsync(leccionId);
        if (lesson?.Modulos is null)
            return Result<CompleteLessonResponseDto>.Failure(["Lección no encontrada."]);

        var cursoId = lesson.Modulos.CursoId;

        var inscription = await inscriptionRepository.GetByUserAndCourseAsync(personId, cursoId);
        if (inscription is null)
            return Result<CompleteLessonResponseDto>.Failure(["Debes estar inscrito en el curso para registrar progreso."]);

        if (inscription.Estado == InscriptionEstate.Cancelado)
            return Result<CompleteLessonResponseDto>.Failure(["La inscripción está cancelada."]);

        var existing = await lessonProgressRepository.GetByUserAndLessonAsync(personId, leccionId);
        if (existing is null)
        {
            await lessonProgressRepository.CreateAsync(new LessonProgress
            {
                UsuarioId = personId,
                LeccionId = leccionId,
                Completado = true,
                FechaCompletado = DateTime.UtcNow,
                EntityStatus = (short)1
            });
        }
        else
        {
            existing.Completado = true;
            existing.FechaCompletado = DateTime.UtcNow;
            await lessonProgressRepository.UpdateAsync(existing);
        }

        var total = await lessonRepository.CountActiveLessonsByCourseAsync(cursoId);
        var progressRows = await lessonProgressRepository.GetByUserAndCourseAsync(personId, cursoId);
        var completadas = progressRows.Count(p => p.Completado == true);

        var pct = total == 0
            ? 100
            : (int)Math.Round(100m * completadas / total);

        if (total > 0 && completadas >= total)
        {
            inscription.Estado = InscriptionEstate.Terminado;
            inscription.FechaCompletado ??= DateTime.UtcNow;
        }
        else if (inscription.Estado == InscriptionEstate.Activo)
        {
            inscription.Estado = InscriptionEstate.Progreso;
        }

        await inscriptionRepository.UpdateAsync(inscription);

        return Result<CompleteLessonResponseDto>.Success(new CompleteLessonResponseDto
        {
            CursoId = cursoId,
            LeccionesCompletadas = completadas,
            LeccionesTotales = total,
            ProgresoPorcentaje = pct,
            CursoCompletado = total > 0 && completadas >= total,
            EstadoInscripcion = inscription.Estado.ToString()
        });
    }
}
