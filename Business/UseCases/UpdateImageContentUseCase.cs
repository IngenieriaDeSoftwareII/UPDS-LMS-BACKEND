using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Entities;
using Data.Repositories.Interfaces;
using FluentValidation;

namespace Business.UseCases;

public class UpdateImageContentUseCase(
    IImageContentRepository repository,
    IValidator<CreateImageContentDto> validator,
    IMapper mapper)
{
    public async Task<Result<ImageContentDto>> ExecuteAsync(int contentId, CreateImageContentDto dto)
    {
        var validation = await validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<ImageContentDto>.Failure(validation.Errors.Select(e => e.ErrorMessage));

        var existing = await repository.GetByContentIdAsync(contentId);
        if (existing is null)
            return Result<ImageContentDto>.Failure(["ImageContent no encontrado"]);

        mapper.Map(dto, existing);

        await repository.UpdateAsync(existing);

        return Result<ImageContentDto>.Success(mapper.Map<ImageContentDto>(existing));
    }
}