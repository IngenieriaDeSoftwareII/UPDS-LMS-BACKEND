using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Repositories.Interfaces;
using Data.Services.Interfaces;
using FluentValidation;

namespace Business.UseCases.Homework;

public class CreateHomeworkUseCase(
    IHomeworkRepository repository,
    IMapper mapper,
    IValidator<CreateHomeworkDto> validator,
    IStorageService storageService)
{
    public async Task<Result<HomeworkDto>> ExecuteAsync(CreateHomeworkDto dto)
    {
        var validation = await validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<HomeworkDto>.Failure(
                validation.Errors.Select(e => e.ErrorMessage)
            );

        var entity = mapper.Map<Data.Entities.Homework>(dto);

        // Ahora sube
        if (dto.File != null)
        {
            using var stream = dto.File.OpenReadStream();

            var fullUrl = await storageService.UploadFileAsync(
                stream,
                dto.File.FileName,
                "homeworks"
            );

            // ✅ EXACTAMENTE igual que submissions
            entity.UrlArchivo = Path.GetFileName(fullUrl);

            entity.TamanoKb = (int)(dto.File.Length / 1024);
        }
        else
        {
            entity.UrlArchivo = null;
        }

        entity.CreatedAt = DateTime.Now;
        entity.UpdatedAt = DateTime.Now;

        var created = await repository.CreateAsync(entity);

        return Result<HomeworkDto>.Success(
            mapper.Map<HomeworkDto>(created)
        );
    }
}