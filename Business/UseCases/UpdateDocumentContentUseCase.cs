using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Entities;
using Data.Repositories.Interfaces;
using FluentValidation;

namespace Business.UseCases;

public class UpdateDocumentContentUseCase(
    IDocumentContentRepository repository,
    IValidator<CreateDocumentContentDto> validator,
    IMapper mapper)
{
    public async Task<Result<DocumentContentDto>> ExecuteAsync(int contentId, CreateDocumentContentDto dto)
    {
        var validation = await validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<DocumentContentDto>.Failure(validation.Errors.Select(e => e.ErrorMessage));

        var existing = await repository.GetByContentIdAsync(contentId);
        if (existing is null)
            return Result<DocumentContentDto>.Failure(["DocumentContent no encontrado"]);

        mapper.Map(dto, existing);

        await repository.UpdateAsync(existing);

        return Result<DocumentContentDto>.Success(mapper.Map<DocumentContentDto>(existing));
    }
}