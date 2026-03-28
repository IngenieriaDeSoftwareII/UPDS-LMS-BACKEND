namespace Business.DTOs.Responses;

public class EvaluationQuestionDto
{
    public int Id { get; set; }
    public int EvaluacionId { get; set; }
    public string Enunciado { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public int Puntos { get; set; }
    public int Orden { get; set; }
    public List<EvaluationAnswerOptionDto> Opciones { get; set; } = [];
}

public class EvaluationAnswerOptionDto
{
    public int Id { get; set; }
    public string Texto { get; set; } = string.Empty;
    public bool EsCorrecta { get; set; }
    public int Orden { get; set; }
}

