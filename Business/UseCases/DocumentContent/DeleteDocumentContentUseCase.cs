using Business.Results;
using Data.Repositories.Interfaces;
using Data.Services.Interfaces;
using System.Threading.Tasks;

namespace Business.UseCases.DocumentContent
{
    public class DeleteDocumentContentUseCase
    {
        private readonly IDocumentContentRepository _repository;
        private readonly IContentRepository _contentRepository;
        private readonly IMediaStorageService _storage; 
        private const string Container = "documents"; 

        public DeleteDocumentContentUseCase(
            IDocumentContentRepository repository,
            IMediaStorageService storage,
            IContentRepository contentRepository)
        {
            _repository = repository;
            _storage = storage;
            _contentRepository = contentRepository;
        }

        public async Task<Result<bool>> ExecuteAsync(int contentId)
        {
            // Obtener DocumentContent existente
            var existing = await _repository.GetByContentIdAsync(contentId);
            if (existing is null)
                return Result<bool>.Failure(new[] { "DocumentContent no encontrado" });

            // Eliminar archivo físico si existe
            if (!string.IsNullOrEmpty(existing.FileUrl))
            {
                await _storage.DeleteAsync(existing.FileUrl, Container);
            }

            // Eliminar registro DocumentContent
            await _repository.DeleteAsync(contentId);

            // Eliminar Content asociado
            if (existing.ContentId != 0)
            {
                await _contentRepository.DeleteAsync(existing.ContentId);
            }

            return Result<bool>.Success(true);
        }
    }
}