namespace Business.DTOs.Requests;

public class AddEvaluationQuestionDto
{
    public int EvaluacionId { get; set; }
    public string Enunciado { get; set; } = string.Empty;
    public string Tipo { get; set; } = "opcion_unica";
    public int Puntos { get; set; } = 1;
    public int Orden { get; set; } = 1;
    public List<AddAnswerOptionDto> Opciones { get; set; } = [];
}

public class AddAnswerOptionDto
{
    public string Texto { get; set; } = string.Empty;
    public bool EsCorrecta { get; set; }
    public int Orden { get; set; } = 1;
}

