namespace Business.DTOs.Responses;

public class ContentDto
{
    public int Id { get; set; }
    public int LessonId { get; set; }
    public string Type { get; set; } = null!;
    public string? Title { get; set; }
    public string? Url { get; set; }
    public int Order { get; set; }
    public int Duration { get; set; }
    public short EntityStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}