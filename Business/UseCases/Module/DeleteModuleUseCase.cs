using Data.Repositories.Interfaces;

namespace Business.UseCases.Modules
{
    public class DeleteModuleUseCase
    {
        private readonly IModuleRepository _repository;

        public DeleteModuleUseCase(IModuleRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Execute(int id)
        {
            var module = await _repository.GetByIdAsync(id);

            if (module == null) return false;

            await _repository.DeleteAsync(module);
            return true;
        }
    }
}