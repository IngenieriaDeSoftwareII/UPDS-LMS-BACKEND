using AutoMapper;
using Business.DTOs.Responses;
using Data.Repositories.Interfaces;

namespace Business.UseCases.Modules
{
    public class GetModuleByIdUseCase
    {
        private readonly IModuleRepository _repository;
        private readonly IMapper _mapper;

        public GetModuleByIdUseCase(IModuleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ModuleDto?> Execute(int id)
        {
            var module = await _repository.GetByIdAsync(id);

            if (module == null) return null;

            return _mapper.Map<ModuleDto>(module);
        }
    }
}