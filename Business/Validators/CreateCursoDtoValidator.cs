using Business.DTOs.Requests;
using FluentValidation;

namespace Business.Validators;

public class CreateCursoDtoValidator : AbstractValidator<CreateCursoDto>
{
    public CreateCursoDtoValidator()
    {
        RuleFor(x => x.Titulo)
            .NotEmpty().WithMessage("El título del curso es requerido.")
            .MaximumLength(150).WithMessage("El título no puede exceder los 150 caracteres.");

        RuleFor(x => x.Nivel)
            .MaximumLength(50).WithMessage("El nivel no puede exceder los 50 caracteres.");
    }
}
