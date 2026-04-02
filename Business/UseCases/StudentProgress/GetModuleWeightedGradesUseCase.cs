using Business.DTOs.Responses;
using Business.Results;
using Data.Repositories.Interfaces;

namespace Business.UseCases.StudentProgress;

public class GetModuleWeightedGradesUseCase(
    IInscriptionRepository inscriptionRepository,
    IGradableItemRepository gradableItemRepository,
    IEvaluationRepository evaluationRepository)
{
    public async Task<Result<IReadOnlyList<ModuleWeightedGradeDto>>> ExecuteAsync(int personId, int cursoId)
    {
        var inscription = await inscriptionRepository.GetByUserAndCourseAsync(personId, cursoId);
        if (inscription is null)
            return Result<IReadOnlyList<ModuleWeightedGradeDto>>.Failure(
                ["Debes estar inscrito en el curso para ver las notas por módulo."]);

        var items = await gradableItemRepository.GetActiveByCourseIdAsync(cursoId);
        var attempts = (await evaluationRepository.GetAttemptsByStudentAsync(personId)).ToList();

        var bestByEval = attempts
            .GroupBy(a => a.EvaluacionId)
            .ToDictionary(g => g.Key, g => g.OrderByDescending(x => x.PuntajeObtenido).First());

        var byModule = items.GroupBy(i => i.ModuloId).OrderBy(g => g.First().Module.Orden);

        var result = new List<ModuleWeightedGradeDto>();

        foreach (var group in byModule)
        {
            var module = group.First().Module;
            decimal acc = 0;
            var itemsEval = 0;
            var itemsCalif = 0;
            decimal ponderacionEval = 0;

            foreach (var item in group)
            {
                if (item.EvaluacionId is not int evId || item.Evaluation is null)
                    continue;

                itemsEval++;
                var maxScore = item.Evaluation.Preguntas.Sum(q => q.Puntos);
                if (maxScore <= 0)
                    continue;

                ponderacionEval += item.Ponderacion;
                decimal pct = 0;
                if (bestByEval.TryGetValue(evId, out var attempt))
                {
                    itemsCalif++;
                    pct = (decimal)(attempt.PuntajeObtenido / maxScore * 100m);
                }

                acc += pct * item.Ponderacion;
            }

            decimal? nota = ponderacionEval > 0
                ? Math.Round(acc / ponderacionEval, 2)
                : null;

            result.Add(new ModuleWeightedGradeDto
            {
                ModuloId = module.Id,
                TituloModulo = module.Titulo,
                Orden = module.Orden,
                NotaPonderada = nota,
                PonderacionTotal = ponderacionEval,
                ItemsConEvaluacion = itemsEval,
                ItemsCalificados = itemsCalif
            });
        }

        return Result<IReadOnlyList<ModuleWeightedGradeDto>>.Success(result);
    }
}
