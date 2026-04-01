namespace Data.Models.Reports.Results;

public class CourseReportRow
{
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int? TeacherId { get; set; }
    public string? TeacherName { get; set; }
    public int TotalEnrollments { get; set; }
    public int TotalCancellations { get; set; }
    public int TotalCompletions { get; set; }
    public decimal CompletionRate { get; set; }
}
