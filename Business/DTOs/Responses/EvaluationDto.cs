namespace Business.DTOs.Responses;

public class EvaluationDto
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
}

