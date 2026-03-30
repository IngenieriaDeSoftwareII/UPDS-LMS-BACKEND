namespace Business.DTOs.Requests;

public class UpdateCatalogDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}
