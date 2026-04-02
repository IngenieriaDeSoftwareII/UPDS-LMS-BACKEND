namespace Business.DTOs.Responses.Reports;

public class TeacherCourseDetailReportDto
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public int TeacherId { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public int CourseId { get; set; }
    public string CourseTitle { get; set; } = string.Empty;
    public int TotalEnrollments { get; set; }
    public int TotalCancellations { get; set; }
    public int TotalCompletions { get; set; }
    public decimal CompletionRate { get; set; }
    public List<ReportMonthCountDto> EnrollmentsByMonth { get; set; } = [];
}

