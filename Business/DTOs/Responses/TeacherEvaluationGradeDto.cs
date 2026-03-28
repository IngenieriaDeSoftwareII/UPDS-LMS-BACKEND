namespace Business.DTOs.Responses;

public class TeacherEvaluationGradeDto
{
    public int EvaluacionId { get; set; }
    public string TituloEvaluacion { get; set; } = string.Empty;
    public int EstudianteId { get; set; }
    public string EstudianteNombreCompleto { get; set; } = string.Empty;
    public int IntentoId { get; set; }
    public int NumeroIntento { get; set; }
    public decimal PuntajeObtenido { get; set; }
    public decimal PuntajeMaximo { get; set; }
    public bool Aprobado { get; set; }
    public DateTime FechaEnvio { get; set; }
}

