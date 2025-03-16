using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SMS.Backend.Data;
using SMS.Models.DTOs.Lessons;

namespace SMS.Backend.Validations.Lessons;

public class UpsertLessonValidation : AbstractValidator<UpsertLessonDto>
{
    private readonly AppDbContext _context;

    public UpsertLessonValidation(AppDbContext context)
    {
        _context = context;

        RuleFor(l => l.Day).IsInEnum().WithMessage("Day is not valid");
        RuleFor(l => l.ClassId).GreaterThan(0).WithMessage("Class Id is required")
            .MustAsync(async (id, cancellation) =>
            {
                return await _context.Classes.AnyAsync(g => g.Id == id, cancellation);
            }).WithMessage("Class id is not valid");
        RuleFor(l => l.SubjectId).GreaterThan(0).WithMessage("Subject Id is required")
            .MustAsync(async (id, cancellation) =>
            {
                return await _context.Subjects.AnyAsync(g => g.Id == id, cancellation);
            }).WithMessage("Subject id is not valid");
        RuleFor(l => l.TeacherId).GreaterThan(0).WithMessage("Teacher Id is required")
            .MustAsync(async (id, cancellation) =>
            {
                return await _context.Teachers.AnyAsync(g => g.Id == id, cancellation);
            }).WithMessage("Teacher id is not valid");
        RuleFor(l => l.StartTime).NotEmpty().WithMessage("Start time is required");
        RuleFor(l => l.EndTime).NotEmpty().WithMessage("End time is required");
    }
}