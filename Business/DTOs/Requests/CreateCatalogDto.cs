namespace Business.DTOs.Requests;

public class CreateCatalogDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}
