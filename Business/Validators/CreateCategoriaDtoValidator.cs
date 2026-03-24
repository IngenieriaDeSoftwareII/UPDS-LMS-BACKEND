using Business.DTOs.Requests;
using FluentValidation;

namespace Business.Validators;

public class CreateCategoriaDtoValidator : AbstractValidator<CreateCategoriaDto>
{
    public CreateCategoriaDtoValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre de la categoría es requerido.")
            .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("El slug es requerido.")
            .MaximumLength(100).WithMessage("El slug no puede exceder los 100 caracteres.");
    }
}
