using Business.DTOs.Requests;
using Data.Enums;
using FluentValidation;

namespace Business.Validators;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("El nombre es requerido.")
            .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("El apellido es requerido.")
            .MaximumLength(100).WithMessage("El apellido no puede superar los 100 caracteres.");

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
