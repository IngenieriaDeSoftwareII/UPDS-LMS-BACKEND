using Business.DTOs.Requests;
using FluentValidation;

namespace Business.Validators;

public class CreateImageContentDtoValidator : AbstractValidator<CreateImageContentDto>
{
    public CreateImageContentDtoValidator()
    {
        RuleFor(x => x.ImageUrl)
            .NotEmpty().WithMessage("La URL de la imagen es requerida.")
            .MaximumLength(500);

        RuleFor(x => x.Format)
            .NotEmpty().WithMessage("El formato es requerido.")
            .MaximumLength(10);

        RuleFor(x => x.WidthPx)
            .GreaterThan(0).WithMessage("El ancho debe ser mayor a 0.")
            .When(x => x.WidthPx.HasValue);

        RuleFor(x => x.HeightPx)
            .GreaterThan(0).WithMessage("La altura debe ser mayor a 0.")
            .When(x => x.HeightPx.HasValue);

        RuleFor(x => x.AltText)
            .NotEmpty().WithMessage("El texto alternativo es requerido.")
            .MaximumLength(200);

        RuleFor(x => x.SizeKb)
            .GreaterThan(0).WithMessage("El tamaño debe ser mayor a 0.")
            .When(x => x.SizeKb.HasValue);
    }
}