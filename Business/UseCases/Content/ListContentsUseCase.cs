using AutoMapper;
using Business.DTOs.Responses;
using Data.Repositories.Interfaces;

namespace Business.UseCases.Content;

public class ListContentsUseCase(IContentRepository repository, IMapper mapper)
{
    public async Task<IEnumerable<ContentDto>> ExecuteAsync()
    {
        var contents = await repository.GetAllAsync();
        return mapper.Map<IEnumerable<ContentDto>>(contents);
    }
}