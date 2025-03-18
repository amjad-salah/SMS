using FluentValidation;
using SMS.Models.DTOs.Institutions;

namespace SMS.Backend.Validations.Institutions;

public class UpsertInstitutionValidation : AbstractValidator<UpsertInstitutionDto>
{
    public UpsertInstitutionValidation()
    {
        RuleFor(i => i.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(i => i.Email).NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email address");
        RuleFor(i => i.Address).NotEmpty().WithMessage("Address is required");
        RuleFor(i => i.Phone).NotEmpty().WithMessage("Phone is required");
        RuleFor(i => i.InstitutionType).IsInEnum().WithMessage("Institution type is required");
    }
}