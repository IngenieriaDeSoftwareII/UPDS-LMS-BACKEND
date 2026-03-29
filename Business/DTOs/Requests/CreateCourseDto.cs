namespace Business.DTOs.Requests;

public class CreateCourseDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? TeacherId { get; set; }
    public string? Level { get; set; }
    public int? CategoryId { get; set; }
    public string? ImageUrl { get; set; }
    public int? MaxStudents { get; set; }
}