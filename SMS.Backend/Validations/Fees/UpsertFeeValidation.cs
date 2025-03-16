using FluentValidation;
using SMS.Models.DTOs.Fees;

namespace SMS.Backend.Validations.Fees;

public class UpsertFeeValidation : AbstractValidator<UpsertFeeDto>
{
    public UpsertFeeValidation()
    {
        RuleFor(f => f.Amount).GreaterThan(0).WithMessage("Amount must be greater than zero.");
        RuleFor(f => f.Type).IsInEnum().WithMessage("Type must be a valid.");
        RuleFor(f => f.Name).NotEmpty().WithMessage("Name is required.");
    }
}