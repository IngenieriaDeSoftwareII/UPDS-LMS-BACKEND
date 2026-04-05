namespace Business.DTOs.Requests;

public class CreateVideoContentDto
{
    public int LessonId { get; set; }   
    public string Title { get; set; } = null!;
    public int Order { get; set; }
    public int DurationSeconds { get; set; }
}