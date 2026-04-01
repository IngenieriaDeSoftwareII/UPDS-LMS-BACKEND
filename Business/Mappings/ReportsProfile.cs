using AutoMapper;
using Business.DTOs.Requests.Reports;
using Business.DTOs.Responses.Reports;
using Data.Reports.Models;

namespace Business.Mappings;

public class ReportsProfile : Profile
{
    public ReportsProfile()
    {
        CreateMap<AdminCoursesReportQueryDto, AdminCoursesReportQuery>();
        CreateMap<AdminTeachersReportQueryDto, AdminTeachersReportQuery>();
        CreateMap<TeacherSummaryReportQueryDto, TeacherSummaryReportQuery>();
        CreateMap<TeacherCoursesReportQueryDto, TeacherCoursesReportQuery>();
        CreateMap<TeacherCourseDetailReportQueryDto, TeacherCourseDetailReportQuery>();

        CreateMap<ReportMonthCount, ReportMonthCountDto>();
        CreateMap<CourseReportRow, CourseReportRowDto>();
        CreateMap<TeacherCourseSummaryRow, TeacherCourseSummaryRowDto>();
        CreateMap<AdminTeacherReportRow, AdminTeacherReportRowDto>();

        CreateMap<AdminCoursesReportResult, AdminCoursesReportDto>();
        CreateMap<AdminTeachersReportResult, AdminTeachersReportDto>();
        CreateMap<TeacherSummaryReportResult, TeacherSummaryReportDto>();
        CreateMap<TeacherCoursesReportResult, TeacherCoursesReportDto>();
        CreateMap<TeacherCourseDetailReportResult, TeacherCourseDetailReportDto>();
    }
}
