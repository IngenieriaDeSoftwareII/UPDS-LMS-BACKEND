using Data.Enums;
using Microsoft.AspNetCore.Http;

namespace Business.DTOs.Requests;

public class CreateHomeworkDto
{
    public int LessonId { get; set; }
    public string Titulo { get; set; } = null!;
    public string? Descripcion { get; set; }
    public DateTime FechaApertura { get; set; }
    public DateTime FechaEntrega { get; set; }
    public DateTime? FechaLimite { get; set; }
    public string? UrlArchivo { get; set; }
    public IFormFile? File { get; set; }
    public FormatDocument? Formato { get; set; }
    public int? TamanoKb { get; set; }
}