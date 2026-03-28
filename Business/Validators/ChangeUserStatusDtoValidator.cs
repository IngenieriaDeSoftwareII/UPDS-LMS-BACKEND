using FluentValidation;

namespace Business.DTOs.Requests;

public class ChangeUserStatusDtoValidator : AbstractValidator<ChangeUserStatusDto>
{
    public ChangeUserStatusDtoValidator()
    {
        RuleFor(x => x.LockedUntil)
            .Must(date => date > DateTimeOffset.UtcNow)
            .WithMessage("La fecha de expiración del bloqueo debe ser en el futuro.")
            .When(x => !x.IsActive && x.LockedUntil.HasValue);
    }
}
