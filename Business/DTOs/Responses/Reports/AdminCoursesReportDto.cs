namespace Business.DTOs.Responses.Reports;

public class AdminCoursesReportDto
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public List<CourseReportRowDto> Courses { get; set; } = [];
    public List<ReportMonthCountDto> EnrollmentsByMonth { get; set; } = [];
}

