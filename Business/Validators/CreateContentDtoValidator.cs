using Business.DTOs.Requests;
using FluentValidation;
using Data.Enums;
namespace Business.Validators;

public class CreateContentDtoValidator : AbstractValidator<CreateContentDto>
{
    public CreateContentDtoValidator()
    {
        RuleFor(x => x.LessonId)
            .GreaterThan(0).WithMessage("El ID de la lección debe ser mayor a 0.");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("El tipo de contenido no es válido.");
        
        RuleFor(x => x.Title)
            .MaximumLength(150).WithMessage("El título no puede exceder los 150 caracteres.");

        RuleFor(x => x.Order)
            .GreaterThanOrEqualTo(0).WithMessage("El orden debe ser mayor o igual a 0.");
    }
}