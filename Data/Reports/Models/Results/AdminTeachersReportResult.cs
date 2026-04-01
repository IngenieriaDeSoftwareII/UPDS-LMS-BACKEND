namespace Data.Reports.Models;

public class AdminTeachersReportResult
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public List<AdminTeacherReportRow> Teachers { get; set; } = [];
}
