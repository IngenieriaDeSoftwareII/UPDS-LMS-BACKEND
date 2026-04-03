using Data.Entities;

namespace Business.Helpers;

public static class CertificateExamRules
{
    public const decimal MinNotaSobre100 = 51m;

    public static decimal ComputeNotaSobre100(Evaluation evaluation, decimal puntajeObtenido)
    {
        var maxFromQuestions = evaluation.Preguntas?.Sum(p => (decimal)p.Puntos) ?? 0m;
        var max = evaluation.PuntajeMaximo ?? maxFromQuestions;
        if (max <= 0)
            max = 100m;

        return Math.Round(puntajeObtenido / max * 100m, 2, MidpointRounding.AwayFromZero);
    }
}
