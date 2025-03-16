using FluentValidation;
using SMS.Models.DTOs.ExamTypes;

namespace SMS.Backend.Validations.ExamTypes;

public class UpsertExamTypeValidation : AbstractValidator<UpsertExamTypeDto>
{
    public UpsertExamTypeValidation()
    {
        RuleFor(t => t.Name).NotEmpty().WithMessage("Name is required.")
            .MaximumLength(20).WithMessage("Name must not exceed 20 characters.");
    }
}