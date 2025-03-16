using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SMS.Backend.Data;
using SMS.Models.DTOs.StudentRecords;

namespace SMS.Backend.Validations.StudentRecords;

public class UpsertStudentRecordValidation : AbstractValidator<UpsertStudentRecordDto>
{
    private readonly AppDbContext _context;

    public UpsertStudentRecordValidation(AppDbContext context)
    {
        _context = context;

        RuleFor(r => r.StudentId).GreaterThan(0).WithMessage("Student Id is required")
            .MustAsync(async (id, cancellation) =>
            {
                return await _context.Students.AnyAsync(g => g.Id == id, cancellation);
            }).WithMessage("Student id is not valid");
        RuleFor(r => r.Remarks).NotEmpty().WithMessage("Remarks is required");
        RuleFor(r => r.Result).IsInEnum().WithMessage("Result is invalid");
        RuleFor(r => r.Notes).NotEmpty().WithMessage("Notes is required");
    }
}