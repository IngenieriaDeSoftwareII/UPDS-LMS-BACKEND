using System.Collections.Generic;

namespace Business.DTOs.Responses;

public class CatalogDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public IEnumerable<CategoryDto> Categorias { get; set; } = new List<CategoryDto>();
}