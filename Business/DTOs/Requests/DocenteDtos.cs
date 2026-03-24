namespace Business.DTOs.Requests;

public class CreateDocenteDto
{
    public string UsuarioId { get; set; } = string.Empty;
    public string? Especialidad { get; set; }
    public string? Biografia { get; set; }
}

public class UpdateDocenteDto
{
    public int Id { get; set; }
    public string? Especialidad { get; set; }
    public string? Biografia { get; set; }
}
