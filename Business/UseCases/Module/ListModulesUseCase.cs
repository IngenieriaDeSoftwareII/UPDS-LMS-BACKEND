using AutoMapper;
using Business.DTOs.Responses;
using Data.Repositories.Interfaces;

namespace Business.UseCases.Modules
{
    public class ListModulesUseCase
    {
        private readonly IModuleRepository _repository;
        private readonly IMapper _mapper;

        public ListModulesUseCase(IModuleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ModuleDto>> Execute()
        {
            var modules = await _repository.GetAllAsync();
            return _mapper.Map<List<ModuleDto>>(modules);
        }
    }
}