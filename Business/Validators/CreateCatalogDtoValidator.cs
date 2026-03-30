using Business.DTOs.Requests;
using FluentValidation;

namespace Business.Validators;

public class CreateCatalogDtoValidator : AbstractValidator<CreateCatalogDto>
{
    public CreateCatalogDtoValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");
    }
}