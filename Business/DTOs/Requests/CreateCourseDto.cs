namespace Business.DTOs.Requests;

public class CreateCourseDto
{
    public string Titulo { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int? DocenteId { get; set; }
    public string? Nivel { get; set; }
    public int? CategoriaId { get; set; }
    public string? ImagenUrl { get; set; }
    public int? MaxEstudiantes { get; set; }
}