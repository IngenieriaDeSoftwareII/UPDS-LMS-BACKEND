namespace Business.DTOs.Responses;

public class CompleteLessonResponseDto
{
    public int CursoId { get; set; }
    public int LeccionesCompletadas { get; set; }
    public int LeccionesTotales { get; set; }
    public int ProgresoPorcentaje { get; set; }
    public bool CursoCompletado { get; set; }
    public string? EstadoInscripcion { get; set; }
}
