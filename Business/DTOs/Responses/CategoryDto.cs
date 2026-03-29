using System.Collections.Generic;

namespace Business.DTOs.Responses;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? CatalogId { get; set; }

    public IEnumerable<CourseDto> Courses { get; set; } = new List<CourseDto>();
}