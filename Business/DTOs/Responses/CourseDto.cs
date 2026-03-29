using System;

namespace Business.DTOs.Responses;

public class CourseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? TeacherId { get; set; }
    public string? Level { get; set; }
    public int? CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public bool IsPublished { get; set; }
    public int TotalDurationMin { get; set; }
    public int? MaxStudents { get; set; }
    public DateTime CreatedAt { get; set; }
}