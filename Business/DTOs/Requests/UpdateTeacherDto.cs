namespace Business.DTOs.Requests;

public class UpdateTeacherDto
{
    public int Id { get; set; }
    public string? Specialty { get; set; }
    public string? Biography { get; set; }
}