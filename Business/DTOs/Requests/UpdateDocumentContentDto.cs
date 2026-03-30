using Microsoft.AspNetCore.Http; 
namespace Business.DTOs.Requests;
public class UpdateDocumentContentDto
{
    public int LessonId { get; set; }
    public string Title { get; set; } = null!;
    public int Order { get; set; }

    public int? PageCount { get; set; }

    // Agregar propiedad para el archivo
    public IFormFile? File { get; set; }

}