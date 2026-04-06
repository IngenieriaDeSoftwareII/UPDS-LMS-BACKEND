using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using AutoMapper;
using Data.Repositories.Interfaces;
using FluentValidation;

namespace Business.UseCases.HomeworkSubmissions;

public class GradeHomeworkSubmissionUseCase(
    IHomeworkSubmissionRepository submissionRepository,
    IMapper mapper,
    IValidator<GradeHomeworkSubmissionDto> validator)
{
    public async Task<Result<HomeworkSubmissionDto>> ExecuteAsync(GradeHomeworkSubmissionDto dto)
    {
        var validation = await validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<HomeworkSubmissionDto>.Failure(validation.Errors.Select(e => e.ErrorMessage));

        var submission = await submissionRepository.GetByIdAsync(dto.SubmissionId);
        if (submission == null) return Result<HomeworkSubmissionDto>.Failure(new[] { "La entrega no existe." });

        submission.Feedback = string.IsNullOrWhiteSpace(dto.Feedback) ? null : dto.Feedback;
        submission.Estado = dto.Revisado ? "revisado" : "sin revisar";
        submission.UpdatedAt = DateTime.Now;

        await submissionRepository.UpdateAsync(submission);

        return Result<HomeworkSubmissionDto>.Success(mapper.Map<HomeworkSubmissionDto>(submission));
    }
}