using AutoMapper;
using Business.DTOs.Responses;
using Data.Repositories.Interfaces;

namespace Business.UseCases;

public class ListDocumentContentsUseCase(IDocumentContentRepository repository, IMapper mapper)
{
    public async Task<IEnumerable<DocumentContentDto>> ExecuteAsync()
    {
        var documents = await repository.GetAllAsync();
        return mapper.Map<IEnumerable<DocumentContentDto>>(documents);
    }
}