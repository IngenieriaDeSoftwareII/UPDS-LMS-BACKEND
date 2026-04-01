namespace Business.DTOs.Responses;

public class TeacherProfileDto
{
    public int TeacherId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Especialidad { get; set; }
    public string? Biografia { get; set; }
}