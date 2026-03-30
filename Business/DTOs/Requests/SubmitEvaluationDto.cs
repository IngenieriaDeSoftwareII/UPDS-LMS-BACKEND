namespace Business.DTOs.Requests;

public class SubmitEvaluationDto
{
    public int EvaluacionId { get; set; }
    public List<SubmitAnswerDto> Respuestas { get; set; } = [];
}

public class SubmitAnswerDto
{
    public int PreguntaId { get; set; }
    public int? OpcionRespuestaId { get; set; }
    public string? RespuestaTexto { get; set; }
}

