using Data.Enums;

namespace Business.DTOs.Responses;

public class HomeworkDto
{
    public int Id { get; set; }
    public int? LessonId { get; set; }
    public string Titulo { get; set; } = null!;
    public string? Descripcion { get; set; }
    public DateTime FechaApertura { get; set; }
    public DateTime FechaEntrega { get; set; }
    public DateTime? FechaLimite { get; set; }
    public string? UrlArchivo { get; set; }
    public FormatDocument? Formato { get; set; }
    public int? TamanoKb { get; set; }
}