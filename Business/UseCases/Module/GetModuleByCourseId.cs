using AutoMapper;
using Business.DTOs.Responses;
using Data.Repositories.Interfaces;

namespace Business.UseCases.Modules;

public class GetModuleByCourseId( IModuleRepository repository, IMapper mapper)
{
    public async Task<IEnumerable<ModuleDto>> ExecuteAsync(int courseId)
    {
        var modules = await repository.GetModulesByCourseIdAsync(courseId);
        return mapper.Map<IEnumerable<ModuleDto>>(modules);
    }
}