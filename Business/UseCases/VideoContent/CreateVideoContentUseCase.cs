using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Entities;
using Data.Repositories.Interfaces;
using FluentValidation;

namespace Business.UseCases.VideoContent;

public class CreateVideoContentUseCase(
    IVideoContentRepository repository,
    IValidator<CreateVideoContentDto> validator,
    IMapper mapper)
{
    public async Task<Result<VideoContentDto>> ExecuteAsync(CreateVideoContentDto dto)
    {
        var validation = await validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<VideoContentDto>.Failure(validation.Errors.Select(e => e.ErrorMessage));

        var entity = mapper.Map<Data.Entities.VideoContent>(dto);
        var created = await repository.CreateAsync(entity);

        return Result<VideoContentDto>.Success(mapper.Map<VideoContentDto>(created));
    }
}