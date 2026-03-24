using Business.DTOs.Requests;
using FluentValidation;

namespace Business.Validators;

public class InscriptionCreateValidator : AbstractValidator<InscriptionCreateRequest>
{
    public InscriptionCreateValidator()
    {
        RuleFor(x => x.UsuarioId).GreaterThan(0);
        RuleFor(x => x.CursoId).GreaterThan(0);
    }
}