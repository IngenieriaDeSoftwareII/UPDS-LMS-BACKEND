using Business.DTOs.Requests;
using FluentValidation;

namespace Business.Validators;

public class SubmitHomeworkDtoValidator : AbstractValidator<SubmitHomeworkDto>
{
    public SubmitHomeworkDtoValidator()
    {
        RuleFor(x => x.HomeworkId)
            .GreaterThan(0);

        // Validar archivo solo si existe
        When(x => x.File != null, () =>
        {
            RuleFor(x => x.File.Length)
                .LessThanOrEqualTo(20 * 1024 * 1024)
                .WithMessage("El archivo no puede pesar más de 20MB.");
        });

    }
}