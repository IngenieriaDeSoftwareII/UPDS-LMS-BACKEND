using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Entities;
using Data.Repositories.Interfaces;
using FluentValidation;

namespace Business.UseCases;

public class CreateDocumentContentUseCase(
    IDocumentContentRepository repository,
    IValidator<CreateDocumentContentDto> validator,
    IMapper mapper)
{
    public async Task<Result<DocumentContentDto>> ExecuteAsync(CreateDocumentContentDto dto)
    {
        var validation = await validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<DocumentContentDto>.Failure(validation.Errors.Select(e => e.ErrorMessage));

        var entity = mapper.Map<DocumentContent>(dto);
        var created = await repository.CreateAsync(entity);

        return Result<DocumentContentDto>.Success(mapper.Map<DocumentContentDto>(created));
    }
}