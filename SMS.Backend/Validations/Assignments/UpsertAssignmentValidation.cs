using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SMS.Backend.Data;
using SMS.Models.DTOs.Assignments;

namespace SMS.Backend.Validations.Assignments;

public class UpsertAssignmentValidation : AbstractValidator<UpsertAssignmentDto>
{
    private readonly AppDbContext _context;

    public UpsertAssignmentValidation(AppDbContext context)
    {
        _context = context;

        RuleFor(a => a.Title).NotEmpty().WithMessage("Title is required")
            .MaximumLength(50).WithMessage("Title must not exceed 50 characters");
        RuleFor(a => a.ClassId).GreaterThan(0).WithMessage("Class Id is required")
            .MustAsync(async (id, cancellation) =>
            {
                return await _context.Classes.AnyAsync(g => g.Id == id, cancellation);
            }).WithMessage("Class id is not valid");
        RuleFor(a => a.StartDate).NotEmpty().WithMessage("Start date is required");
        RuleFor(a => a.EndDate).NotEmpty().WithMessage("End date is required");
    }
}