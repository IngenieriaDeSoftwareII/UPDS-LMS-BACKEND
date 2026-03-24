using Business.DTOs.Requests;
using FluentValidation;

namespace Business.Validators;

public class CreateVideoContentDtoValidator : AbstractValidator<CreateVideoContentDto>
{
    public CreateVideoContentDtoValidator()
    {
        RuleFor(x => x.VideoUrl)
            .NotEmpty().WithMessage("La URL del video es requerida.")
            .MaximumLength(500);

        RuleFor(x => x.DurationSeconds)
            .GreaterThan(0).WithMessage("La duración debe ser mayor a 0.");
    }
}