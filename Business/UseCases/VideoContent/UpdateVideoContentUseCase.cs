using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Entities;
using Data.Repositories.Interfaces;
using FluentValidation;

namespace Business.UseCases.VideoContent;

public class UpdateVideoContentUseCase(
    IVideoContentRepository repository,
    IValidator<CreateVideoContentDto> validator,
    IMapper mapper)
{
    public async Task<Result<VideoContentDto>> ExecuteAsync(int contentId, CreateVideoContentDto dto)
    {
        var validation = await validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<VideoContentDto>.Failure(validation.Errors.Select(e => e.ErrorMessage));

        var existing = await repository.GetByContentIdAsync(contentId);
        if (existing is null)
            return Result<VideoContentDto>.Failure(["VideoContent no encontrado"]);

        mapper.Map(dto, existing);

        await repository.UpdateAsync(existing);

        return Result<VideoContentDto>.Success(mapper.Map<VideoContentDto>(existing));
    }
}