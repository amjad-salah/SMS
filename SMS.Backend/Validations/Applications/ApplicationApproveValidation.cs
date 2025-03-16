using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SMS.Backend.Data;
using SMS.Models.DTOs.Applications;

namespace SMS.Backend.Validations.Applications;

public class ApplicationApproveValidation : AbstractValidator<ApplicationApproveDto>
{
    private readonly AppDbContext _context;

    public ApplicationApproveValidation(AppDbContext context)
    {
        _context = context;

        RuleFor(a => a.ClassId).GreaterThan(0).WithMessage("Class id is required.")
            .MustAsync(async (id, cancellation) =>
            {
                return await _context.Classes.AnyAsync(c => c.Id == id, cancellation);
            }).WithMessage("Class id is invalid.");
        RuleFor(a => a.Tax).GreaterThanOrEqualTo(0).WithMessage("Tax is required.");
        RuleFor(a => a.Discount).GreaterThanOrEqualTo(0).WithMessage("Discount is required.");
    }
}