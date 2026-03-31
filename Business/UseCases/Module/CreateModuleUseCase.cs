using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Data.Entities;
using Data.Repositories.Interfaces;

namespace Business.UseCases.Modules
{
    public class CreateModuleUseCase
    {
        private readonly IModuleRepository _repository;
        private readonly IMapper _mapper;

        public CreateModuleUseCase(IModuleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ModuleDto> Execute(CreateModuleDto dto)
        {
            var module = _mapper.Map<Module>(dto);
            module.CreatedAt = DateTime.UtcNow;

            await _repository.AddAsync(module);

            return _mapper.Map<ModuleDto>(module);
        }
    }
}