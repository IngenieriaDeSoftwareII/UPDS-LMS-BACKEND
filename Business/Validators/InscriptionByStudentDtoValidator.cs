using Business.DTOs.Requests;
using FluentValidation;

namespace Business.Validators;

public class InscriptionByStudentDtoValidator : AbstractValidator<InscriptionByStudentDto>
{
    public InscriptionByStudentDtoValidator()
    {
        RuleFor(x => x.UsuarioId).GreaterThan(0).WithMessage("El ID del usuario debe ser mayor a 0.");
    }
}