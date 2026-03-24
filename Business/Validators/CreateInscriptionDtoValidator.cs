using Business.DTOs.Requests;
using FluentValidation;

namespace Business.Validators;

public class CreateInscriptionDtoValidator : AbstractValidator<CreateInscriptionDto>
{
    public CreateInscriptionDtoValidator()
    {
        RuleFor(x => x.UsuarioId).GreaterThan(0);
        RuleFor(x => x.CursoId).GreaterThan(0);
    }
}