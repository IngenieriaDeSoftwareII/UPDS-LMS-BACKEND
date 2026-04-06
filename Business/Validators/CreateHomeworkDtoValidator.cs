using Business.DTOs.Requests;
using FluentValidation;

namespace Business.Validators;

public class CreateHomeworkDtoValidator : AbstractValidator<CreateHomeworkDto>
{
    public CreateHomeworkDtoValidator()
    {
        RuleFor(x => x.Titulo)
            .NotEmpty()
            .WithMessage("El título es obligatorio")
            .MaximumLength(150);

        RuleFor(x => x.FechaApertura).NotEmpty();

        RuleFor(x => x.FechaEntrega)
            .GreaterThan(x => x.FechaApertura)
            .WithMessage("La fecha de entrega debe ser posterior a la fecha de apertura.");
    }
}