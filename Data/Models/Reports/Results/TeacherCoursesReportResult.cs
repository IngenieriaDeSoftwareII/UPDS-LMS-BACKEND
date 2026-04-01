namespace Data.Models.Reports.Results;

public class TeacherCoursesReportResult
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public int TeacherId { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public List<TeacherCourseSummaryRow> Courses { get; set; } = [];
}
