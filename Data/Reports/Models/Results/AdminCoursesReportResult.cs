namespace Data.Reports.Models;

public class AdminCoursesReportResult
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public List<CourseReportRow> Courses { get; set; } = [];
    public List<ReportMonthCount> EnrollmentsByMonth { get; set; } = [];
}
