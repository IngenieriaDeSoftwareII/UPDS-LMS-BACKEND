namespace Data.Reports.Models;

public class TeacherSummaryReportResult
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public int TeacherId { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public int TotalCourses { get; set; }
    public int TotalEnrollments { get; set; }
    public int TotalCancellations { get; set; }
    public int TotalCompletions { get; set; }
    public decimal CompletionRate { get; set; }
    public List<ReportMonthCount> EnrollmentsByMonth { get; set; } = [];
}
