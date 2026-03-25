using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Entities;
using Data.Enums;
using Data.Repositories.Interfaces;
using FluentValidation;

namespace Business.UseCases
{
    public class CreateInscriptionUseCase(
        IInscriptionRepository repository,
        ICursoRepository cursoRepository,
        IMapper mapper,
        IValidator<CreateInscriptionDto> validator)
    {
        public async Task<Result<InscriptionDto>> ExecuteAsync(CreateInscriptionDto dto)
        {
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return Result<InscriptionDto>.Failure(validation.Errors.Select(e => e.ErrorMessage));

            var curso = await cursoRepository.GetByIdAsync(dto.CursoId);
            if (curso is null)
                return Result<InscriptionDto>.Failure(["El curso no existe."]);

            if (curso.MaxEstudiantes is int limite && limite > 0)
            {
                var inscriptosActivos = await repository.CountActiveInscriptionsByCourseAsync(dto.CursoId);
                if (inscriptosActivos >= limite)
                    return Result<InscriptionDto>.Failure(
                        ["No hay cupos disponibles para este curso."]);
            }

            var existing = await repository.GetByUserAndCourseAsync(
                dto.UsuarioId,
                dto.CursoId);

            if (existing is not null && existing.Estado != InscriptionEstate.Cancelado)
                return Result<InscriptionDto>.Failure(["El usuario ya está inscrito en este curso."]);

            Inscription saved;
            if (existing is not null)
            {
                existing.Estado = InscriptionEstate.Activo;
                existing.DeletedAt = null;
                saved = await repository.UpdateAsync(existing);
            }
            else
            {
                var inscription = mapper.Map<Inscription>(dto);
                saved = await repository.CreateAsync(inscription);
            }

            var resultDto = mapper.Map<InscriptionDto>(saved);
            resultDto.Curso = mapper.Map<CourseDto>(curso);
            return Result<InscriptionDto>.Success(resultDto);
        }
    }
}
