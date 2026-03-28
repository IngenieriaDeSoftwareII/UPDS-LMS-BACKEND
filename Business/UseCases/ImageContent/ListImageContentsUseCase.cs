using AutoMapper;
using Business.DTOs.Responses;
using Data.Repositories.Interfaces;

namespace Business.UseCases.ImageContent;
public class ListImageContentsUseCase(IImageContentRepository repository, IMapper mapper)
{
    public async Task<IEnumerable<ImageContentDto>> ExecuteAsync()
    {
        var images = await repository.GetAllAsync();
        return mapper.Map<IEnumerable<ImageContentDto>>(images);
    }
}