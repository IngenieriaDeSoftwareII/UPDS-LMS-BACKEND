using Business.DTOs.Requests;
using FluentValidation;

namespace Business.Validators;

public class CreateCourseDtoValidator : AbstractValidator<CreateCourseDto>
{
    public CreateCourseDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required").MaximumLength(150);
    }
}