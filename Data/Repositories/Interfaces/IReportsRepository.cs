using Data.Reports.Models;

namespace Data.Repositories.Interfaces;

public interface IReportsRepository
{
    Task<AdminCoursesReportResult> GetAdminCoursesAsync(AdminCoursesReportQuery query);
    Task<AdminTeachersReportResult> GetAdminTeachersAsync(AdminTeachersReportQuery query);

    Task<TeacherSummaryReportResult> GetTeacherSummaryAsync(string teacherUserId, TeacherSummaryReportQuery query);
    Task<TeacherCoursesReportResult> GetTeacherCoursesAsync(string teacherUserId, TeacherCoursesReportQuery query);
    Task<TeacherCourseDetailReportResult> GetTeacherCourseDetailAsync(string teacherUserId, int courseId, TeacherCourseDetailReportQuery query);
}

