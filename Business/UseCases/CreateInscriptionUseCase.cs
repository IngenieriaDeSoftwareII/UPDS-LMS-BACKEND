using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Entities;
using Data.Repositories.Interfaces;
using FluentValidation;

namespace Business.UseCases
{
    public class CreateInscriptionUseCase(
        IInscriptionRepository repository,
        IMapper mapper,
        IValidator<CreateInscriptionDto> validator)
    {
        public async Task<Result<InscriptionDto>> ExecuteAsync(CreateInscriptionDto dto)
        {
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return Result<InscriptionDto>.Failure(validation.Errors.Select(e => e.ErrorMessage));

            var inscription = mapper.Map<Inscription>(dto);

            var verifyDuplicate = await repository.GetByUserAndCourseAsync(
                inscription.UsuarioId,
                inscription.CursoId);

            if (verifyDuplicate != null)
                return Result<InscriptionDto>.Failure(["El usuario ya está inscrito en este curso."]);

            var created = await repository.CreateAsync(inscription);

            var resultDto = mapper.Map<InscriptionDto>(created);
            return Result<InscriptionDto>.Success(resultDto);
        }
    }
}
