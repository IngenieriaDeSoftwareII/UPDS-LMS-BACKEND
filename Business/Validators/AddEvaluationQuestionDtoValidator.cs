using Business.DTOs.Requests;
using FluentValidation;

namespace Business.Validators;

public class AddEvaluationQuestionDtoValidator : AbstractValidator<AddEvaluationQuestionDto>
{
    public AddEvaluationQuestionDtoValidator()
    {
        RuleFor(x => x.EvaluacionId).GreaterThan(0);
        RuleFor(x => x.Enunciado).NotEmpty();
        RuleFor(x => x.Tipo).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Puntos).GreaterThan(0);

        RuleFor(x => x.Opciones)
            .NotNull()
            .Must(o => o.Count >= 2)
            .WithMessage("Debes enviar al menos 2 opciones.");

        RuleFor(x => x.Opciones)
            .Must(o => o.Count(op => op.EsCorrecta) == 1)
            .WithMessage("Debe existir exactamente 1 opción correcta.");

        RuleForEach(x => x.Opciones).ChildRules(option =>
        {
            option.RuleFor(o => o.Texto).NotEmpty();
        });
    }
}

