namespace Data.Reports.Models;

public class AdminTeacherReportRow
{
    public int TeacherId { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public int TotalCourses { get; set; }
    public int TotalEnrollments { get; set; }
    public int TotalCancellations { get; set; }
    public int TotalCompletions { get; set; }
    public decimal CompletionRate { get; set; }
    public List<ReportMonthCount> EnrollmentsByMonth { get; set; } = [];
    public List<TeacherCourseSummaryRow> Courses { get; set; } = [];
}
