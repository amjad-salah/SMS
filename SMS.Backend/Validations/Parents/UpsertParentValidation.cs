using FluentValidation;
using SMS.Models.DTOs.Parents;

namespace SMS.Backend.Validations.Parents;

public class UpsertParentValidation : AbstractValidator<UpsertParentDto>
{
    public UpsertParentValidation()
    {
        RuleFor(p => p.FullName).NotEmpty().WithMessage("Full name is required")
            .MaximumLength(100).WithMessage("Full name must not exceed 100 characters");
        RuleFor(p => p.Address).NotEmpty().WithMessage("Address is required")
            .MaximumLength(100).WithMessage("Address must not exceed 100 characters");
        RuleFor(p => p.Email).EmailAddress().WithMessage("Email is required")
            .MaximumLength(100).WithMessage("Email must not exceed 100 characters");
        RuleFor(p => p.Phone).NotEmpty().WithMessage("Phone is required")
            .MaximumLength(20).WithMessage("Phone must not exceed 20 characters");
    }
}