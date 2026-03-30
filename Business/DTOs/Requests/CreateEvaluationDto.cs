namespace Business.DTOs.Requests;

public class CreateEvaluationDto
{
    public int CursoId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string Tipo { get; set; } = "multiple_opcion";
    public decimal? PuntajeMaximo { get; set; }
    public decimal? PuntajeMinimoAprobacion { get; set; }
    public int IntentosPermitidos { get; set; } = 1;
    public int? TiempoLimiteMax { get; set; }
}

