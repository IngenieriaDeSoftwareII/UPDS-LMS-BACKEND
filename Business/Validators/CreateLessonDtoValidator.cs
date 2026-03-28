using Business.DTOs.Requests;
using FluentValidation;

namespace Business.Validators;

public class CreateLessonDtoValidator : AbstractValidator<CreateLessonDto>
{
    public CreateLessonDtoValidator()
    {
        RuleFor(x => x.ModuleId)
            .GreaterThan(0).WithMessage("El ID del módulo debe ser mayor a 0.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("El título es requerido.")
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .When(x => x.Description is not null);

        RuleFor(x => x.Order)
            .GreaterThanOrEqualTo(0).WithMessage("El orden debe ser mayor o igual a 0.");
    }
}