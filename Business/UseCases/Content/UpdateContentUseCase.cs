using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Entities;
using Data.Repositories.Interfaces;
using FluentValidation;

namespace Business.UseCases.Content;

public class UpdateContentUseCase(
    IContentRepository repository,
    IValidator<CreateContentDto> validator,
    IMapper mapper)
{
    public async Task<Result<ContentDto>> ExecuteAsync(int id, CreateContentDto dto)
    {
        var validation = await validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<ContentDto>.Failure(validation.Errors.Select(e => e.ErrorMessage));

        var existing = await repository.GetByIdAsync(id);
        if (existing is null)
            return Result<ContentDto>.Failure(["Content no encontrado"]);

        // Mapear cambios
        mapper.Map(dto, existing);

        await repository.UpdateAsync(existing);

        return Result<ContentDto>.Success(mapper.Map<ContentDto>(existing));
    }
}