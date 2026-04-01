using AutoMapper;
using Business.DTOs.Requests.Reports;
using Business.DTOs.Responses.Reports;
using Business.Results;
using Data.Models.Reports.Queries;
using Data.Repositories.Interfaces;

namespace Business.UseCases.Reports;

public class GetAdminCoursesReportUseCase(IReportsRepository repository, IMapper mapper)
{
    public async Task<Result<AdminCoursesReportDto>> ExecuteAsync(AdminCoursesReportQueryDto query)
    {
        var data = await repository.GetAdminCoursesAsync(mapper.Map<AdminCoursesReportQuery>(query));
        return Result<AdminCoursesReportDto>.Success(mapper.Map<AdminCoursesReportDto>(data));
    }
}
