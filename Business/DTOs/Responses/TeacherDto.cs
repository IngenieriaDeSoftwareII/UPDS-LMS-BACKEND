namespace Business.DTOs.Responses;

public class TeacherDto
{
    public int Id { get; set; }
    public string UsuarioId { get; set; } = string.Empty;
    public string? Especialidad { get; set; }
    public string? Biografia { get; set; }
    public int TotalCursos { get; set; }
    public DateTime CreatedAt { get; set; }

    public string NombreCompleto { get; set; } = string.Empty; // Mapped from Person

    public IEnumerable<CourseDto> Cursos { get; set; } = new List<CourseDto>();
}