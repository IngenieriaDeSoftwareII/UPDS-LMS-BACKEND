namespace Data.Models.Reports.Queries;

public class TeacherCoursesReportQuery
{
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public bool? Published { get; set; }
}
