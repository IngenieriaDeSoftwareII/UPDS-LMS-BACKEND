namespace Business.DTOs.Responses;

public class EvaluationSubmissionResultDto
{
    public int EvaluacionId { get; set; }
    public int IntentoId { get; set; }
    public int NumeroIntento { get; set; }
    public decimal PuntajeObtenido { get; set; }
    public decimal PuntajeMaximo { get; set; }
    public bool Aprobado { get; set; }
}

