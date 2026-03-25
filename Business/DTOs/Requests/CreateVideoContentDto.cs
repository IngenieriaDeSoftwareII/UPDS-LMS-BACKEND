namespace Business.DTOs.Requests;

public class CreateVideoContentDto
{
    public int ContentId { get; set; }
    public string VideoUrl { get; set; } = null!;
    public int DurationSeconds { get; set; }
}