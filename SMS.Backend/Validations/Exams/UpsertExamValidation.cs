using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SMS.Backend.Data;
using SMS.Models.DTOs.Exams;

namespace SMS.Backend.Validations.Exams;

public class UpsertExamValidation : AbstractValidator<UpsertExamDto>
{
    private readonly AppDbContext _context;

    public UpsertExamValidation(AppDbContext context)
    {
        _context = context;

        RuleFor(e => e.Title).NotEmpty().WithMessage("Title is required.")
            .MaximumLength(50).WithMessage("Title must not exceed 50 characters.");
        RuleFor(e => e.GradeId).GreaterThan(0).WithMessage("Grade Id is required.")
            .MustAsync(async (id, cancellation) =>
            {
                return await _context.Grades.AnyAsync(g => g.Id == id, cancellation);
            }).WithMessage("Grade id is not valid");
        RuleFor(e => e.ExamDate).NotEmpty().WithMessage("Exam Date is required.");
        RuleFor(e => e.StartTime).NotEmpty().WithMessage("Start time is required.");
        RuleFor(e => e.EndTime).NotEmpty().WithMessage("End time is required.");
        RuleFor(e => e.MaxMark).NotEmpty().WithMessage("Max Mark is required.");
        RuleFor(e => e.MinMark).NotEmpty().WithMessage("Min Mark is required.");
        RuleFor(e => e.AcademicYearId).GreaterThan(0).WithMessage("Academic Year Id is required.")
            .MustAsync(async (id, cancellation) =>
            {
                return await _context.AcademicYears.AnyAsync(g => g.Id == id, cancellation);
            }).WithMessage("Academic year id is not valid");
        ;
        RuleFor(e => e.ExamTypeId).GreaterThan(0).WithMessage("Exam Type Id is required.")
            .MustAsync(async (id, cancellation) =>
            {
                return await _context.ExamTypes.AnyAsync(g => g.Id == id, cancellation);
            }).WithMessage("Exam type id is not valid");
    }
}