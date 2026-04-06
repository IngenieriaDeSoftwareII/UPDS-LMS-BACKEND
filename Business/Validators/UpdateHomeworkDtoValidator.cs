using Business.DTOs.Requests;
using FluentValidation;

namespace Business.Validators;

public class UpdateHomeworkDtoValidator : AbstractValidator<UpdateHomeworkDto>
{
    public UpdateHomeworkDtoValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Titulo).NotEmpty().MaximumLength(150);
        
        RuleFor(x => x.FechaApertura).NotEmpty();

        RuleFor(x => x.FechaEntrega)
            .GreaterThan(x => x.FechaApertura)
            .WithMessage("La fecha de entrega debe ser posterior a la fecha de apertura.");

    }
}