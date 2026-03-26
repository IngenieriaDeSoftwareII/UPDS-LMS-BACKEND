using Business.DTOs.Requests;
using Data.Enums;
using FluentValidation;

namespace Business.Validators;

public class UpdatePersonDtoValidator : AbstractValidator<UpdatePersonDto>
{
    private static readonly string[] ValidExpeditions =
        ["LP", "CB", "SC", "OR", "PT", "TJ", "BN", "PD", "CH"];

    public UpdatePersonDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("El nombre no puede estar vacío si se proporciona.")
            .MaximumLength(100)
            .When(x => x.FirstName != null);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("El apellido paterno no puede estar vacío si se proporciona.")
            .MaximumLength(100)
            .When(x => x.LastName != null);

        RuleFor(x => x.MotherLastName)
            .NotEmpty().WithMessage("El apellido materno no puede estar vacío si se proporciona.")
            .MaximumLength(100)
            .When(x => x.MotherLastName != null);

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("La fecha de nacimiento no puede estar vacía si se proporciona.")
            .Must(d => d < DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("La fecha de nacimiento debe ser anterior a hoy.")
            .Must(d => d <= DateOnly.FromDateTime(DateTime.Today.AddYears(-16)))
                .WithMessage("La persona debe tener al menos 16 años.")
            .When(x => x.DateOfBirth.HasValue);

        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage("El género especificado no es válido.")
            .When(x => x.Gender.HasValue);

        RuleFor(x => x.NationalId)
            .NotEmpty().WithMessage("El número de carnet no puede estar vacío si se proporciona.")
            .MaximumLength(20)
            .When(x => x.NationalId != null);

        RuleFor(x => x.NationalIdExpedition)
            .NotEmpty().WithMessage("El lugar de expedición no puede estar vacío si se proporciona.")
            .Must(v => ValidExpeditions.Contains(v!.ToUpper()))
                .WithMessage($"El lugar de expedición debe ser uno de: {string.Join(", ", ValidExpeditions)}.")
            .When(x => x.NationalIdExpedition != null);

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20)
            .When(x => x.PhoneNumber != null);

        RuleFor(x => x.Address)
            .MaximumLength(255)
            .When(x => x.Address != null);
    }
}
