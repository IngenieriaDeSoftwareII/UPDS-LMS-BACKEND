using AutoMapper;
using Business.DTOs.Responses;
using Data.Repositories.Interfaces;

namespace Business.UseCases.Homework;

public class GetHomeworkSubmissionsUseCase(
    IHomeworkSubmissionRepository submissionRepository,
    IMapper mapper
)
{
    public async Task<IEnumerable<HomeworkSubmissionDto>> ExecuteAsync(int homeworkId)
    {
        var submissions = await submissionRepository.GetByHomeworkIdAsync(homeworkId);
        return mapper.Map<IEnumerable<HomeworkSubmissionDto>>(submissions);
    }
}