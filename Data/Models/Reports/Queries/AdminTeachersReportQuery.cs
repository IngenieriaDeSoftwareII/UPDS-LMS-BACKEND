namespace Data.Models.Reports.Queries;

public class AdminTeachersReportQuery
{
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public int? CategoryId { get; set; }
}
