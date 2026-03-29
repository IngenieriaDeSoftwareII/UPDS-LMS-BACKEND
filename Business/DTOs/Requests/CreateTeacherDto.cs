namespace Business.DTOs.Requests;

public class CreateTeacherDto
{
    public string UserId { get; set; } = string.Empty;
    public string? Specialty { get; set; }
    public string? Biography { get; set; }
}