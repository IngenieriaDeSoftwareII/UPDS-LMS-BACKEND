using Business.DTOs.Requests;
using Data.Enums;
using FluentValidation;

namespace Business.Validators;

public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email no puede estar vacío si se proporciona.")
            .EmailAddress().WithMessage("El formato del correo institucional no es válido.")
            .When(x => x.Email != null);

        RuleFor(x => x.Role)
            .Must(role => UserRoles.All.Contains(role!))
            .WithMessage($"El rol debe ser uno de los siguientes: {string.Join(", ", UserRoles.All)}.")
            .When(x => x.Role != null);
    }
}
