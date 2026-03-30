using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Entities;
using Data.Repositories.Interfaces;
using FluentValidation;

namespace Business.UseCases.Content;

public class CreateContentUseCase(
    IContentRepository contentRepository,
    IValidator<CreateContentDto> contentValidator,
    IMapper mapper)
{
    public async Task<Result<ContentDto>> ExecuteAsync(CreateContentDto dto)
    {
        var validation = await contentValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<ContentDto>.Failure(validation.Errors.Select(e => e.ErrorMessage));

        var content = mapper.Map<Data.Entities.Content>(dto);
        var createdContent = await contentRepository.CreateAsync(content);

        return Result<ContentDto>.Success(mapper.Map<ContentDto>(createdContent));
    }
}