namespace Business.DTOs.Requests;

public class CreateCategoriaDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}
