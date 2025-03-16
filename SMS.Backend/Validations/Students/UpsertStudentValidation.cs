using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SMS.Backend.Data;
using SMS.Models.DTOs.Students;

namespace SMS.Backend.Validations.Students;

public class UpsertStudentValidation : AbstractValidator<UpsertStudentDto>
{
    private readonly AppDbContext _context;

    public UpsertStudentValidation(AppDbContext context)
    {
        _context = context;

        RuleFor(s => s.InstitutionId).GreaterThan(0).WithMessage("Institution Id is required.");
        RuleFor(s => s.GradeId).GreaterThan(0).WithMessage("Grade Id is required.")
            .MustAsync(async (id, cancellation) =>
            {
                return await _context.Grades.AnyAsync(g => g.Id == id, cancellation);
            }).WithMessage("Grade id is not valid");
        RuleFor(s => s.ClassId).GreaterThan(0).WithMessage("Class Id is required.")
            .MustAsync(async (id, cancellation) =>
            {
                return await _context.Classes.AnyAsync(g => g.Id == id, cancellation);
            }).WithMessage("Class id is not valid");
        RuleFor(s => s.FullName).NotEmpty().WithMessage("Full name is required.")
            .MaximumLength(255).WithMessage("Full name cannot exceed 255 characters.");
        RuleFor(s => s.ParentId).GreaterThan(0).WithMessage("Parent Id is required.")
            .MustAsync(async (id, cancellation) =>
            {
                return await _context.Parents.AnyAsync(g => g.Id == id, cancellation);
            }).WithMessage("Parent id is not valid");
        RuleFor(s => s.AcademicYearId).GreaterThan(0).WithMessage("Academic Year Id is required.")
            .MustAsync(async (id, cancellation) =>
            {
                return await _context.AcademicYears.AnyAsync(g => g.Id == id, cancellation);
            }).WithMessage("Academic year id is not valid");
        RuleFor(s => s.Gender).IsInEnum().WithMessage("Gender is required.");
        RuleFor(s => s.BirthDate).NotEmpty().WithMessage("Birth Date is required.");
        RuleFor(s => s.Status).IsInEnum().WithMessage("Status is required.");
        RuleFor(s => s.AdmissionDate).NotEmpty().WithMessage("Admission Date is required.");
    }
}