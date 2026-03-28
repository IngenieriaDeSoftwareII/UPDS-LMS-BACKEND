using System.Collections.Generic;

namespace Business.DTOs.Responses;

public class DocenteDto
{
    public int Id { get; set; }
    public string UsuarioId { get; set; } = string.Empty;
    public string? Especialidad { get; set; }
    public string? Biografia { get; set; }
    public int TotalCursos { get; set; }
    
    public IEnumerable<CursoDto> Cursos { get; set; } = new List<CursoDto>();
}
