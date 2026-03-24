namespace Business.DTOs.Requests;

public class CreateContentDto
{
    public int LessonId { get; set; }
    public string Type { get; set; } = null!;
    public string? Title { get; set; }
    public string? Url { get; set; }
    public int Order { get; set; } = 1;
    public int Duration { get; set; }
}