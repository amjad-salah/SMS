using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SMS.Backend.Data;
using SMS.Models.DTOs.Attendances;

namespace SMS.Backend.Validations.Attendances;

public class AddClassAttendancesValidation : AbstractValidator<AddClassAttendancesDto>
{
    private readonly AppDbContext _context;

    public AddClassAttendancesValidation(AppDbContext context)
    {
        _context = context;

        RuleFor(a => a.ClassId).GreaterThan(0).WithMessage("Class Id is required")
            .MustAsync(async (id, cancellation) =>
            {
                return await _context.Classes.AnyAsync(g => g.Id == id, cancellation);
            }).WithMessage("Class id is not valid");
    }
}