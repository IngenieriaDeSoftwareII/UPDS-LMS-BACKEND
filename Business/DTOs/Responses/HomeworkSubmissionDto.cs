using Data.Enums;

namespace Business.DTOs.Responses;

public class HomeworkSubmissionDto
{
    public int Id { get; set; }
    public int HomeworkId { get; set; }
    public string HomeworkTitulo { get; set; } = null!;
    public int UsuarioId { get; set; }
    public string? UrlArchivo { get; set; }
    public FormatDocument? Formato { get; set; }
    public int? TamanoKb { get; set; }
    public bool Revisado { get; set; }
    public string? Comentario { get; set; }
    public DateTime FechaEntrega { get; set; }
    public string? Feedback { get; set; }
    public string Estado { get; set; } = null!;
    public string EstudianteNombre { get; set; } = null!;
}