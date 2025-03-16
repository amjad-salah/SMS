using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SMS.Backend.Data;
using SMS.Models.DTOs.Attendances;

namespace SMS.Backend.Validations.Attendances;

public class UpsertAttendanceValidation : AbstractValidator<UpsertAttendanceDto>
{
    private readonly AppDbContext _context;

    public UpsertAttendanceValidation(AppDbContext context)
    {
        _context = context;

        RuleFor(a => a.ClassId).GreaterThan(0).WithMessage("Class Id is required")
            .MustAsync(async (id, cancellation) =>
            {
                return await _context.Classes.AnyAsync(g => g.Id == id, cancellation);
            }).WithMessage("Class id is not valid");
        RuleFor(a => a.Date).NotEmpty().WithMessage("Date is required");
        RuleFor(a => a.Present).NotEmpty().WithMessage("Present is required");
        RuleFor(a => a.StudentId).GreaterThan(0).WithMessage("Student Id is required")
            .MustAsync(async (id, cancellation) =>
            {
                return await _context.Students.AnyAsync(g => g.Id == id, cancellation);
            }).WithMessage("Student id is not valid");
    }
}