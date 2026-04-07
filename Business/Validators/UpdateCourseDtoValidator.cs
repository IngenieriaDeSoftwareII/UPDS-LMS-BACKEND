using Business.DTOs.Requests;
using FluentValidation;

namespace Business.Validators;

public class UpdateCourseDtoValidator : AbstractValidator<UpdateCourseDto>
{
    public UpdateCourseDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("El ID del curso es requerido y debe ser mayor a 0");

        RuleFor(x => x.Titulo)
            .NotEmpty().WithMessage("El título es requerido")
            .MaximumLength(150).WithMessage("El título no puede exceder 150 caracteres");

        RuleFor(x => x.DuracionTotalMin)
            .GreaterThanOrEqualTo(0).WithMessage("La duración debe ser mayor o igual a 0");

        RuleFor(x => x.MaxEstudiantes)
            .GreaterThanOrEqualTo(1).When(x => x.MaxEstudiantes.HasValue)
            .WithMessage("El máximo de estudiantes debe ser mayor a 0");
    }
}
