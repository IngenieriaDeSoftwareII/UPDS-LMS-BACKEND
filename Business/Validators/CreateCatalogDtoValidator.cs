using Business.DTOs.Requests;
using FluentValidation;

namespace Business.Validators;

public class CreateCatalogDtoValidator : AbstractValidator<CreateCatalogDto>
{
    public CreateCatalogDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");
    }
}