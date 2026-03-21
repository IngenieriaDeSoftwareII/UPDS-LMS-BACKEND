using Business.DTOs.Requests;
using Data.Enums;
using FluentValidation;

namespace Business.Validators;

public class CreatePersonDtoValidator : AbstractValidator<CreatePersonDto>
{
    private static readonly string[] ValidExpeditions =
        ["LP", "CB", "SC", "OR", "PT", "TJ", "BN", "PD", "CH"];

    public CreatePersonDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("El nombre es requerido.")
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("El apellido paterno es requerido.")
            .MaximumLength(100);

        RuleFor(x => x.MotherLastName)
            .NotEmpty().WithMessage("El apellido materno es requerido.")
            .MaximumLength(100);

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("La fecha de nacimiento es requerida.")
            .Must(d => d < DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("La fecha de nacimiento debe ser anterior a hoy.")
            .Must(d => d <= DateOnly.FromDateTime(DateTime.Today.AddYears(-16)))
                .WithMessage("La persona debe tener al menos 16 años.");

        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage("El género especificado no es válido.");

        RuleFor(x => x.NationalId)
            .NotEmpty().WithMessage("El número de carnet es requerido.")
            .MaximumLength(20);

        RuleFor(x => x.NationalIdExpedition)
            .NotEmpty().WithMessage("El lugar de expedición es requerido.")
            .Must(v => ValidExpeditions.Contains(v.ToUpper()))
                .WithMessage($"El lugar de expedición debe ser uno de: {string.Join(", ", ValidExpeditions)}.");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20)
            .When(x => x.PhoneNumber is not null);

        RuleFor(x => x.Address)
            .MaximumLength(255)
            .When(x => x.Address is not null);
    }
}
