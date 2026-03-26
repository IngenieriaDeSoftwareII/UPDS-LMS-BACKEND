namespace Business.DTOs.Requests;

public class CreateLessonDto
{
    public int CourseId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int Order { get; set; }
}