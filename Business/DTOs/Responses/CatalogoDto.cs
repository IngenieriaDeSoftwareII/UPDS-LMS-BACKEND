using System.Collections.Generic;

namespace Business.DTOs.Responses;

public class CatalogoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }

    public IEnumerable<CategoriaDto> Categorias { get; set; } = new List<CategoriaDto>();
}