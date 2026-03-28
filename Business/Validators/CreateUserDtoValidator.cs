using Business.DTOs.Requests;
using Data.Enums;
using FluentValidation;

namespace Business.Validators;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.PersonId)
            .GreaterThan(0).WithMessage("El Id de persona es requerido.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo institucional es requerido.")
            .EmailAddress().WithMessage("El formato del correo no es válido.")
            .MaximumLength(256).WithMessage("El correo no puede superar los 256 caracteres.");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("El rol es requerido.")
            .Must(role => UserRoles.All.Contains(role))
            .WithMessage($"El rol debe ser uno de los siguientes: {string.Join(", ", UserRoles.All)}.");
    }
}
