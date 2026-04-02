using AutoMapper;
using Business.DTOs.Requests.Reports;
using Business.DTOs.Responses.Reports;
using Business.Results;
using Data.Models.Reports.Queries;
using Data.Repositories.Interfaces;

namespace Business.UseCases.Reports;

public class GetTeacherCourseDetailReportUseCase(IReportsRepository repository, IMapper mapper)
{
    public async Task<Result<TeacherCourseDetailReportDto>> ExecuteAsync(string teacherUserId, int courseId, TeacherCourseDetailReportQueryDto query)
    {
        var data = await repository.GetTeacherCourseDetailAsync(teacherUserId, courseId, mapper.Map<TeacherCourseDetailReportQuery>(query));
        return Result<TeacherCourseDetailReportDto>.Success(mapper.Map<TeacherCourseDetailReportDto>(data));
    }
}
