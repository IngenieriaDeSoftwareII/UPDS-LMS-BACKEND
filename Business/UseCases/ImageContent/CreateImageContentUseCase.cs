using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Entities;
using Data.Repositories.Interfaces;
using FluentValidation;

namespace Business.UseCases.ImageContent;

public class CreateImageContentUseCase(
    IImageContentRepository repository,
    IValidator<CreateImageContentDto> validator,
    IMapper mapper)
{
    public async Task<Result<ImageContentDto>> ExecuteAsync(CreateImageContentDto dto)
    {
        var validation = await validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<ImageContentDto>.Failure(validation.Errors.Select(e => e.ErrorMessage));

        var entity = mapper.Map<Data.Entities.ImageContent>(dto);
        var created = await repository.CreateAsync(entity);

        return Result<ImageContentDto>.Success(mapper.Map<ImageContentDto>(created));
    }
}