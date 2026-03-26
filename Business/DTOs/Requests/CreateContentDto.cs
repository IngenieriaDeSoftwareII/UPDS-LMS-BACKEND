using Data.Enums;

namespace Business.DTOs.Requests;

public class CreateContentDto
{
    public int LessonId { get; set; }
    public TypeContent Type { get; set; }
    public string? Title { get; set; }
    public int Order { get; set; } = 1;
}