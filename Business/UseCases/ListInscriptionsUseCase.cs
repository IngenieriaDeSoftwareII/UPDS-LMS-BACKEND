using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Repositories.Interfaces;
using FluentValidation;

namespace Business.UseCases
{
    public class ListInscriptionsUseCase(
        IInscriptionRepository repository,
        IMapper mapper,
        IValidator<InscriptionByStudentDto> validator)
    {
        public async Task<Result<IEnumerable<InscriptionDto>>> ExecuteAsync(InscriptionByStudentDto dto)
        {
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return Result<IEnumerable<InscriptionDto>>.Failure(
                    validation.Errors.Select(e => e.ErrorMessage));

            var inscriptions = await repository.GetByUserAsync(dto.UsuarioId);

            var result = mapper.Map<IEnumerable<InscriptionDto>>(inscriptions);
            return Result<IEnumerable<InscriptionDto>>.Success(result);
        }
    }
}
