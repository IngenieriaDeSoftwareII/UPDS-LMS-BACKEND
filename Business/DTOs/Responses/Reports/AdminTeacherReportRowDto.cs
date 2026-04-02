namespace Business.DTOs.Responses.Reports;

public class AdminTeacherReportRowDto
{
    public int TeacherId { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public int TotalCourses { get; set; }
    public int TotalEnrollments { get; set; }
    public int TotalCancellations { get; set; }
    public int TotalCompletions { get; set; }
    public decimal CompletionRate { get; set; }
    public List<ReportMonthCountDto> EnrollmentsByMonth { get; set; } = [];
    public List<TeacherCourseSummaryRowDto> Courses { get; set; } = [];
}

