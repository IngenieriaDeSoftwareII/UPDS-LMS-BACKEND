namespace Business.DTOs.Requests;

public class CreateVideoContentDto
{
    public string VideoUrl { get; set; } = null!;
    public int DurationSeconds { get; set; }
}