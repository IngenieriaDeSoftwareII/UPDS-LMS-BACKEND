using Business.DTOs.Requests;
using FluentValidation;

namespace Business.Validators;

public class CreateContentDtoValidator : AbstractValidator<CreateContentDto>
{
    public CreateContentDtoValidator()
    {
        RuleFor(x => x.LessonId)
            .GreaterThan(0).WithMessage("El ID de la lección debe ser mayor a 0.");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("El tipo de contenido es requerido.")
            .MaximumLength(50);

        RuleFor(x => x.Title)
            .MaximumLength(200)
            .When(x => x.Title is not null);

        RuleFor(x => x.Url)
            .MaximumLength(500)
            .When(x => x.Url is not null);

        RuleFor(x => x.Order)
            .GreaterThanOrEqualTo(0).WithMessage("El orden debe ser mayor o igual a 0.");

        RuleFor(x => x.Duration)
            .GreaterThanOrEqualTo(0).WithMessage("La duración debe ser mayor o igual a 0.");
    }
}