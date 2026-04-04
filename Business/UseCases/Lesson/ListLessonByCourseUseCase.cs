using AutoMapper;
using Business.DTOs.Responses;
using Data.Repositories.Interfaces;

namespace Business.UseCases.Lesson;
public class ListLessonByCourseUseCase(ILessonRepository repository, IMapper mapper)
{
    public async Task<IEnumerable<LessonWithContentsDto>> ExecuteAsync(int moduleId, int courseId)
    {
        var lessons = await repository.GetLessonsByCourseAndModuleAsync(courseId, moduleId);
        return mapper.Map<IEnumerable<LessonWithContentsDto>>(lessons); 
    }
}