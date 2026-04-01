using Business.DTOs.Responses.Reports;

namespace Business.Services.Reports;

public interface IReportExportService
{
    ReportExportResult ExportAdminCourses(AdminCoursesReportDto report, ReportExportFormat format);
    ReportExportResult ExportAdminTeachers(AdminTeachersReportDto report, ReportExportFormat format);
    ReportExportResult ExportTeacherSummary(TeacherSummaryReportDto report, ReportExportFormat format);
    ReportExportResult ExportTeacherCourses(TeacherCoursesReportDto report, ReportExportFormat format);
    ReportExportResult ExportTeacherCourseDetail(TeacherCourseDetailReportDto report, ReportExportFormat format);
}

