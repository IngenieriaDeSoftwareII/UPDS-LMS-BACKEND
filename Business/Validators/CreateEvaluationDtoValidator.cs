using Business.DTOs.Requests;
using FluentValidation;

namespace Business.Validators;

public class CreateEvaluationDtoValidator : AbstractValidator<CreateEvaluationDto>
{
    public CreateEvaluationDtoValidator()
    {
        RuleFor(x => x.CursoId).GreaterThan(0);
        RuleFor(x => x.Titulo).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Tipo).NotEmpty().MaximumLength(50);
        RuleFor(x => x.IntentosPermitidos).GreaterThan(0);
    }
}

