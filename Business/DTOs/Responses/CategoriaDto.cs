using System.Collections.Generic;

namespace Business.DTOs.Responses;

public class CategoriaDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int? CatalogoId { get; set; }

    public IEnumerable<CursoDto> Cursos { get; set; } = new List<CursoDto>();
}
