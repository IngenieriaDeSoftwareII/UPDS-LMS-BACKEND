using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Entities;
using Data.Repositories.Interfaces;
using FluentValidation;

namespace Business.UseCases.Lesson;

public class UpdateLessonUseCase(
    ILessonRepository repository,
    IValidator<CreateLessonDto> validator,
    IMapper mapper)
{
    public async Task<Result<LessonDto>> ExecuteAsync(int id, CreateLessonDto dto)
    {
        var validation = await validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<LessonDto>.Failure(validation.Errors.Select(e => e.ErrorMessage));

        var existing = await repository.GetByIdAsync(id);
        if (existing is null)
            return Result<LessonDto>.Failure(["Lesson no encontrada"]);

        mapper.Map(dto, existing);

        await repository.UpdateAsync(existing);

        return Result<LessonDto>.Success(mapper.Map<LessonDto>(existing));
    }
}