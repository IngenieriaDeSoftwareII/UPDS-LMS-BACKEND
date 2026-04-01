namespace Business.DTOs.Responses.Reports;

public class AdminTeachersReportDto
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public List<AdminTeacherReportRowDto> Teachers { get; set; } = [];
}

