using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Repositories.Interfaces;
using Data.Services.Interfaces;
using FluentValidation;

namespace Business.UseCases.Homework;

public class UpdateHomeworkUseCase(
    IHomeworkRepository repository,
    IMapper mapper,
    IValidator<UpdateHomeworkDto> validator,
    IStorageService storageService)
{
    public async Task<Result<HomeworkDto>> ExecuteAsync(UpdateHomeworkDto dto)
    {
        var validation = await validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<HomeworkDto>.Failure(validation.Errors.Select(e => e.ErrorMessage));

        var entity = await repository.GetByIdAsync(dto.Id);
        if (entity == null)
            return Result<HomeworkDto>.Failure(["No existe"]);

        mapper.Map(dto, entity);

        if (dto.File != null)
        {
            if (!string.IsNullOrEmpty(entity.UrlArchivo))
                await storageService.DeleteFileAsync(entity.UrlArchivo, "homeworks");

            using var stream = dto.File.OpenReadStream();

            entity.UrlArchivo = await storageService.UploadFileAsync(
                stream,
                dto.File.FileName,
                "homeworks"
            );
        }

        entity.UpdatedAt = DateTime.Now;

        await repository.UpdateAsync(entity);

        return Result<HomeworkDto>.Success(mapper.Map<HomeworkDto>(entity));
    }
}