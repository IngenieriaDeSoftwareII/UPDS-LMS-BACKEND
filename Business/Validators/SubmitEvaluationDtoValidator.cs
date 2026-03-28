using Business.DTOs.Requests;
using FluentValidation;

namespace Business.Validators;

public class SubmitEvaluationDtoValidator : AbstractValidator<SubmitEvaluationDto>
{
    public SubmitEvaluationDtoValidator()
    {
        RuleFor(x => x.EvaluacionId).GreaterThan(0);
        RuleFor(x => x.Respuestas)
            .NotNull()
            .Must(r => r.Count > 0)
            .WithMessage("Debes enviar al menos una respuesta.");
    }
}

