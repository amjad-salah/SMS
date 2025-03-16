using FluentValidation;
using SMS.Models.DTOs.Grades;

namespace SMS.Backend.Validations.Grades;

public class UpsertGradeValidation : AbstractValidator<UpsertGradeDto>
{
    public UpsertGradeValidation()
    {
        RuleFor(g => g.Name).NotEmpty().WithMessage("Name is required.")
            .MaximumLength(255).WithMessage("Name must not exceed 255 characters.");
    }
}