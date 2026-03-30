namespace Business.DTOs.Responses;

public class CatalogDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }

    public IEnumerable<CategoryDto> Categorias { get; set; } = new List<CategoryDto>();
}