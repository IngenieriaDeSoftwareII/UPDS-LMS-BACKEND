using Business.DTOs.Requests;
using FluentValidation;

namespace Business.Validators;

public class CreateTeacherDtoValidator : AbstractValidator<CreateTeacherDto>
{
    public CreateTeacherDtoValidator()
    {
        RuleFor(x => x.UsuarioId).NotEmpty().WithMessage("El UsuarioId es requerido.");
    }
}