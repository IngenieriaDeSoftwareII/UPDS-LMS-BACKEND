namespace Business.DTOs.Requests;

public class CreateCatalogDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
