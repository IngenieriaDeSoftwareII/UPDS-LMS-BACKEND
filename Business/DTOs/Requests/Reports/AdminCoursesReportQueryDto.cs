namespace Business.DTOs.Requests.Reports;

public class AdminCoursesReportQueryDto
{
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public int? CategoryId { get; set; }
    public int? TeacherId { get; set; }
    public bool? Published { get; set; }
}

