using Business.DTOs.Requests;
using FluentValidation;

namespace Business.Validators;

public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
{
    public CreateCategoryDtoValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre es requerido").MaximumLength(100);
        RuleFor(x => x.Slug).NotEmpty().WithMessage("El slug es requerido").MaximumLength(100);
    }
}