using Business.DTOs.Requests;
using FluentValidation;

namespace Business.Validators;

public class CancelInscriptionDtoValidator : AbstractValidator<CancelInscriptionDto>
{
    public CancelInscriptionDtoValidator()
    {
        RuleFor(x => x.UsuarioId).GreaterThan(0);
        RuleFor(x => x.CursoId).GreaterThan(0);
    }
}
