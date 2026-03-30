using Business.DTOs.Requests;
using FluentValidation;

namespace Business.Validators;

public class CreateDocumentContentDtoValidator : AbstractValidator<CreateDocumentContentDto>
{
    public CreateDocumentContentDtoValidator()
    {
        RuleFor(x => x.FileUrl)
            .NotEmpty().WithMessage("La URL del archivo es requerida.")
            .MaximumLength(500);

        RuleFor(x => x.Format)
            .NotEmpty().WithMessage("El formato es requerido.")
            .MaximumLength(10);

        RuleFor(x => x.SizeKb)
            .GreaterThan(0).WithMessage("El tamaño debe ser mayor a 0.")
            .When(x => x.SizeKb.HasValue);

        RuleFor(x => x.PageCount)
            .GreaterThan(0).WithMessage("El número de páginas debe ser mayor a 0.")
            .When(x => x.PageCount.HasValue);
    }
}