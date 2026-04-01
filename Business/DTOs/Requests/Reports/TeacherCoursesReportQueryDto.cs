namespace Business.DTOs.Requests.Reports;

public class TeacherCoursesReportQueryDto
{
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public bool? Published { get; set; }
}

