using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Context;
using Data.Entities;
using Data.Repositories.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Business.UseCases.Lesson;

public class CreateLessonUseCase(
    ILessonRepository repository,
    IMapper mapper,
    IValidator<CreateLessonDto> validator,
    AppDbContext context)
{
    public async Task<Result<LessonDto>> ExecuteAsync(CreateLessonDto dto)
    {
        var validation = await validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<LessonDto>.Failure(validation.Errors.Select(e => e.ErrorMessage));

        //Verificar que el módulo exista en la base de datos
        var moduleExists = await context.Modules.AnyAsync(m => m.Id == dto.ModuleId);
        if (!moduleExists)
            return Result<LessonDto>.Failure(["El módulo con el ID especificado no existe."]);

        var lesson = mapper.Map<Data.Entities.Lesson>(dto);
        var created = await repository.CreateAsync(lesson);

        return Result<LessonDto>.Success(mapper.Map<LessonDto>(created));
    }
}