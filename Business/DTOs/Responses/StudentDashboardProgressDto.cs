namespace Business.DTOs.Responses;

public class StudentDashboardProgressDto
{
    public int CursoId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? ImagenUrl { get; set; }
    public string EstadoInscripcion { get; set; } = string.Empty;
    public DateTime? FechaCompletado { get; set; }
    public int ProgresoPorcentaje { get; set; }
    public int LeccionesCompletadas { get; set; }
    public int LeccionesTotales { get; set; }
}
