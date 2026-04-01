using AutoMapper;
using Business.DTOs.Requests.Reports;
using Business.DTOs.Responses.Reports;
using Business.Results;
using Data.Reports.Models;
using Data.Repositories.Interfaces;

namespace Business.UseCases.Reports;

public class GetTeacherSummaryReportUseCase(IReportsRepository repository, IMapper mapper)
{
    public async Task<Result<TeacherSummaryReportDto>> ExecuteAsync(string teacherUserId, TeacherSummaryReportQueryDto query)
    {
        var data = await repository.GetTeacherSummaryAsync(teacherUserId, mapper.Map<TeacherSummaryReportQuery>(query));
        return Result<TeacherSummaryReportDto>.Success(mapper.Map<TeacherSummaryReportDto>(data));
    }
}
