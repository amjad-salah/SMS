using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SMS.Backend.Data;
using SMS.Models.DTOs.ExamResults;

namespace SMS.Backend.Validations.ExamResults;

public class UpsertExamResultsValidation : AbstractValidator<UpsertExamResultDto>
{
    private readonly AppDbContext _context;

    public UpsertExamResultsValidation(AppDbContext context)
    {
        _context = context;

        RuleFor(r => r.StudentId).GreaterThan(0).WithMessage("Student Id is required")
            .MustAsync(async (id, cancellation) =>
            {
                return await _context.Students.AnyAsync(s => s.Id == id, cancellation);
            }).WithMessage("Student id is not valid");
        RuleFor(r => r.Score).NotEmpty().WithMessage("Score is required");
        RuleFor(r => r.ExamId).GreaterThan(0).WithMessage("Exam Id is required")
            .MustAsync(async (id, cancellation) =>
            {
                return await _context.Exams.AnyAsync(g => g.Id == id, cancellation);
            }).WithMessage("Exam id is not valid");
    }
}