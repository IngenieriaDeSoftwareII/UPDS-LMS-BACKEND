namespace Business.DTOs.Responses.Reports;

public class TeacherCoursesReportDto
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public int TeacherId { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public List<TeacherCourseSummaryRowDto> Courses { get; set; } = [];
}

