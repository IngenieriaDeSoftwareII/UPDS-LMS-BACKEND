namespace Business.DTOs.Requests.Reports;

public class AdminTeachersReportQueryDto
{
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public int? CategoryId { get; set; }
}

