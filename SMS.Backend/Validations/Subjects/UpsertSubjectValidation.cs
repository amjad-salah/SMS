using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SMS.Backend.Data;
using SMS.Models.DTOs.Subjects;

namespace SMS.Backend.Validations.Subjects;

public class UpsertSubjectValidation : AbstractValidator<UpsertSubjectDto>
{
    private readonly AppDbContext _context;

    public UpsertSubjectValidation(AppDbContext context)
    {
        _context = context;

        RuleFor(s => s.Name).Empty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
        RuleFor(s => s.GradeId).NotEqual(0).WithMessage("Grade Id is required.")
            .MustAsync(async (id, cancellation) =>
            {
                return await _context.Grades.AnyAsync(g => g.Id == id, cancellation);
            }).WithMessage("Grade id is not valid");
    }
}