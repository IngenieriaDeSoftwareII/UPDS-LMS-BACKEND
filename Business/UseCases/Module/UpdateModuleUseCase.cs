using AutoMapper;
using Business.DTOs.Requests;
using Data.Repositories.Interfaces;

namespace Business.UseCases.Modules
{
    public class UpdateModuleUseCase
    {
        private readonly IModuleRepository _repository;
        private readonly IMapper _mapper;

        public UpdateModuleUseCase(IModuleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<bool> Execute(UpdateModuleDto dto)
        {
            var module = await _repository.GetByIdAsync(dto.Id);

            if (module == null) return false;

            _mapper.Map(dto, module);
            module.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(module);

            return true;
        }
    }
}