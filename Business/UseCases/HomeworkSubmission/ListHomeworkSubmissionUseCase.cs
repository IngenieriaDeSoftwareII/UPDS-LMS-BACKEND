using AutoMapper;
using Business.DTOs.Responses;
using Data.Repositories.Interfaces;

namespace Business.UseCases.HomeworkSubmissions;

public class ListHomeworkSubmissionUseCase
{
    private readonly IHomeworkSubmissionRepository _repository;
    private readonly IMapper _mapper;

    public ListHomeworkSubmissionUseCase(IHomeworkSubmissionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    //Obtener todas las entregas
    public async Task<IEnumerable<HomeworkSubmissionDto>> ExecuteAsync()
    {
        var entities = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<HomeworkSubmissionDto>>(entities);
    }

    // Obtener entrega por Id
    public async Task<HomeworkSubmissionDto?> GetByIdAsync(int submissionId)
    {
        var entity = await _repository.GetByIdAsync(submissionId);
        if (entity == null) return null;
        return _mapper.Map<HomeworkSubmissionDto>(entity);
    }
}