using Business.DTOs.Requests;
using FluentValidation;

namespace Business.Validators;

public class CreateCourseDtoValidator : AbstractValidator<CreateCourseDto>
{
    public CreateCourseDtoValidator()
    {
        RuleFor(x => x.Titulo).NotEmpty().WithMessage("El título es requerido").MaximumLength(150);
    }
}