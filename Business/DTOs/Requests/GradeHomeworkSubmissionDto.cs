namespace Business.DTOs.Requests;

public class GradeHomeworkSubmissionDto
{
    public int SubmissionId { get; set; }
    public bool Revisado { get; set; }
    public string? Feedback { get; set; }
}