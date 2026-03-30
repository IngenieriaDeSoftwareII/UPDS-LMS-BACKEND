namespace Business.DTOs.Requests;

public class CreateTeacherDto
{
    public string UsuarioId { get; set; } = string.Empty;
    public string? Especialidad { get; set; }
    public string? Biografia { get; set; }
}