using AutoMapper;
using Business.DTOs.Requests.Reports;
using Business.DTOs.Responses.Reports;
using Business.Results;
using Data.Reports.Models;
using Data.Repositories.Interfaces;

namespace Business.UseCases.Reports;

public class GetTeacherCoursesReportUseCase(IReportsRepository repository, IMapper mapper)
{
    public async Task<Result<TeacherCoursesReportDto>> ExecuteAsync(string teacherUserId, TeacherCoursesReportQueryDto query)
    {
        var data = await repository.GetTeacherCoursesAsync(teacherUserId, mapper.Map<TeacherCoursesReportQuery>(query));
        return Result<TeacherCoursesReportDto>.Success(mapper.Map<TeacherCoursesReportDto>(data));
    }
}
