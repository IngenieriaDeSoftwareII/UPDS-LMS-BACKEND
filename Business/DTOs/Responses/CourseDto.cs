namespace Business.DTOs.Responses;

public class CourseDto
{
    public int Id { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public int? DocenteId { get; set; }

    public string? Nivel { get; set; }

    public int? CategoriaId { get; set; }

    public string CategoriaNombre { get; set; } = string.Empty;

    public string? ImagenUrl { get; set; }

    public bool Publicado { get; set; }

    public int DuracionTotalMin { get; set; }

    public int? MaxEstudiantes { get; set; }

    public DateTime CreatedAt { get; set; }
}