using Business.DTOs.Requests;
using FluentValidation;

namespace Business.Validators;

public class CreateVideoContentDtoValidator : AbstractValidator<CreateVideoContentDto>
{
    public CreateVideoContentDtoValidator()
    {
        RuleFor(x => x.LessonId)
            .GreaterThan(0)
            .WithMessage("Debes seleccionar una lección válida.");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("El título es obligatorio.")
            .MaximumLength(200);

        RuleFor(x => x.Order)
            .GreaterThan(0)
            .WithMessage("El orden debe ser mayor a 0.");

        RuleFor(x => x.DurationSeconds)
            .GreaterThan(0)
            .WithMessage("La duración debe ser mayor a 0.");
    }
}