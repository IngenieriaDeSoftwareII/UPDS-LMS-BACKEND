using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Enums;
using Data.Repositories.Interfaces;
using FluentValidation;

namespace Business.UseCases;

public class CancelInscriptionUseCase(
    IInscriptionRepository repository,
    ICursoRepository cursoRepository,
    IMapper mapper,
    IValidator<CancelInscriptionDto> validator)
{
    public async Task<Result<InscriptionDto>> ExecuteAsync(CancelInscriptionDto dto)
    {
        var validation = await validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<InscriptionDto>.Failure(validation.Errors.Select(e => e.ErrorMessage));

        var existing = await repository.GetByUserAndCourseAsync(dto.UsuarioId, dto.CursoId);
        if (existing is null)
        {
            return Result<InscriptionDto>.Failure(
                ["No hay una inscripción a este curso para el usuario indicado."]);
        }

        if (existing.Estado == InscriptionEstate.Cancelado)
        {
            return Result<InscriptionDto>.Failure(
                ["La inscripción ya está cancelada."]);
        }

        if (existing.Estado == InscriptionEstate.Terminado)
        {
            return Result<InscriptionDto>.Failure(
                ["No se puede cancelar una inscripción ya finalizada."]);
        }

        existing.Estado = InscriptionEstate.Cancelado;
        var saved = await repository.UpdateAsync(existing);

        var curso = await cursoRepository.GetByIdAsync(dto.CursoId);
        var resultDto = mapper.Map<InscriptionDto>(saved);
        resultDto.Curso = curso is null
            ? new CourseDto { Id = dto.CursoId }
            : mapper.Map<CourseDto>(curso);

        return Result<InscriptionDto>.Success(resultDto);
    }
}
