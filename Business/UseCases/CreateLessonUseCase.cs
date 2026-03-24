using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Entities;
using Data.Repositories.Interfaces;
using FluentValidation;

namespace Business.UseCases;

public class CreateLessonUseCase(
    ILessonRepository repository,
    IMapper mapper,
    IValidator<CreateLessonDto> validator)
{
    public async Task<Result<LessonDto>> ExecuteAsync(CreateLessonDto dto)
    {
        var validation = await validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<LessonDto>.Failure(validation.Errors.Select(e => e.ErrorMessage));

        var lesson = mapper.Map<Lesson>(dto);
        var created = await repository.CreateAsync(lesson);

        return Result<LessonDto>.Success(mapper.Map<LessonDto>(created));
    }
}