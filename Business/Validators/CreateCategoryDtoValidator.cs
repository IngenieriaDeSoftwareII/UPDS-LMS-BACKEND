using System;
using Business.DTOs.Requests;
using FluentValidation;

namespace Business.Validators;

public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
{
    public CreateCategoryDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required").MaximumLength(100);
        RuleFor(x => x.Slug).NotEmpty().WithMessage("Slug is required").MaximumLength(100);
    }
} 