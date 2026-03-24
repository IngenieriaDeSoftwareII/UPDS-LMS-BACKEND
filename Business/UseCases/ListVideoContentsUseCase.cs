using AutoMapper;
using Business.DTOs.Responses;
using Data.Repositories.Interfaces;

namespace Business.UseCases;

public class ListVideoContentsUseCase(IVideoContentRepository repository, IMapper mapper)
{
    public async Task<IEnumerable<VideoContentDto>> ExecuteAsync()
    {
        var videos = await repository.GetAllAsync();
        return mapper.Map<IEnumerable<VideoContentDto>>(videos);
    }
}