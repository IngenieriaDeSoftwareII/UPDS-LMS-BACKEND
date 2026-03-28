namespace Business.DTOs.Responses;

public class EvaluationToTakeDto
{
    public int Id { get; set; }
    public int CursoId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public decimal? PuntajeMaximo { get; set; }
    public decimal? PuntajeMinimoAprobacion { get; set; }
    public int IntentosPermitidos { get; set; }
    public int? TiempoLimiteMax { get; set; }
    public List<EvaluationQuestionToTakeDto> Preguntas { get; set; } = [];
}

public class EvaluationQuestionToTakeDto
{
    public int Id { get; set; }
    public int EvaluacionId { get; set; }
    public string Enunciado { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public int Puntos { get; set; }
    public int Orden { get; set; }
    public List<EvaluationAnswerOptionToTakeDto> Opciones { get; set; } = [];
}

public class EvaluationAnswerOptionToTakeDto
{
    public int Id { get; set; }
    public string Texto { get; set; } = string.Empty;
    public int Orden { get; set; }
}

