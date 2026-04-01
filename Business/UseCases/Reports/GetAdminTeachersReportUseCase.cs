using AutoMapper;
using Business.DTOs.Requests.Reports;
using Business.DTOs.Responses.Reports;
using Business.Results;
using Data.Reports.Models;
using Data.Repositories.Interfaces;

namespace Business.UseCases.Reports;

public class GetAdminTeachersReportUseCase(IReportsRepository repository, IMapper mapper)
{
    public async Task<Result<AdminTeachersReportDto>> ExecuteAsync(AdminTeachersReportQueryDto query)
    {
        var data = await repository.GetAdminTeachersAsync(mapper.Map<AdminTeachersReportQuery>(query));
        return Result<AdminTeachersReportDto>.Success(mapper.Map<AdminTeachersReportDto>(data));
    }
}
