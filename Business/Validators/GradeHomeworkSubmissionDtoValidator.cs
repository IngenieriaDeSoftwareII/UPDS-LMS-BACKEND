using Business.DTOs.Requests;
using FluentValidation;

namespace Business.Validators;

public class GradeHomeworkSubmissionDtoValidator : AbstractValidator<GradeHomeworkSubmissionDto>
{
    public GradeHomeworkSubmissionDtoValidator()
    {
        RuleFor(x => x.SubmissionId).GreaterThan(0);

    }
}