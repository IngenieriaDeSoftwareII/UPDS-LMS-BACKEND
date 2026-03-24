using AutoMapper;
using Business.DTOs.Responses;
using Data.Repositories.Interfaces;

namespace Business.UseCases;

public class ListLessonsUseCase(ILessonRepository repository, IMapper mapper)
{
    public async Task<IEnumerable<LessonDto>> ExecuteAsync()
    {
        var lessons = await repository.GetAllAsync();
        return mapper.Map<IEnumerable<LessonDto>>(lessons);
    }
}