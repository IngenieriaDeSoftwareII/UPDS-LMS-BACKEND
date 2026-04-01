namespace Data.Reports.Models;

public class TeacherCourseSummaryRow
{
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool Published { get; set; }
    public DateTime CreatedAt { get; set; }
    public int TotalEnrollments { get; set; }
    public int TotalCancellations { get; set; }
    public int TotalCompletions { get; set; }
    public decimal CompletionRate { get; set; }
}
