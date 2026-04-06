using AutoMapper;
using Business.DTOs.Responses;
using Data.Repositories.Interfaces;

namespace Business.UseCases.Homework;

public class ListHomeworkUseCase(IHomeworkRepository repository, IMapper mapper)
{
    public async Task<IEnumerable<HomeworkDto>> ExecuteAsync()
    {
        var entities = await repository.GetAllAsync();
        return mapper.Map<IEnumerable<HomeworkDto>>(entities);
    }
}