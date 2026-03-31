using Business.DTOs.Requests;
using FluentValidation;

namespace Business.Validators
{
    public class CreateModuleDtoValidator : AbstractValidator<CreateModuleDto>
    {
        public CreateModuleDtoValidator()
        {
            RuleFor(x => x.Titulo)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.CursoId)
                .GreaterThan(0);

            RuleFor(x => x.Orden)
                .GreaterThanOrEqualTo(0);
        }
    }

    public class UpdateModuleDtoValidator : AbstractValidator<UpdateModuleDto>
    {
        public UpdateModuleDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);

            RuleFor(x => x.Titulo)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.CursoId)
                .GreaterThan(0);

            RuleFor(x => x.Orden)
                .GreaterThanOrEqualTo(0);
        }
    }
}