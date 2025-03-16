using FluentValidation;
using SMS.Models.DTOs.AcademicYears;

namespace SMS.Backend.Validations.AcademicYears;

public class UpsertAcademicYearValidation : AbstractValidator<UpsertAcademicYearDto>
{
    public UpsertAcademicYearValidation()
    {
        RuleFor(a => a.StartDate).NotEmpty().WithMessage("Start date is required.");
        RuleFor(a => a.EndDate).NotEmpty().WithMessage("End date is required.");
        RuleFor(a => a.Name).NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");
    }
}