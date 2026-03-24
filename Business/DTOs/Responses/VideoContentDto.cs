namespace Business.DTOs.Responses;

public class VideoContentDto
{
    public int ContentId { get; set; }
    public string VideoUrl { get; set; } = null!;
    public int DurationSeconds { get; set; }
}