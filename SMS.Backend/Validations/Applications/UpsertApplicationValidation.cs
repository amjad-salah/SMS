using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SMS.Backend.Data;
using SMS.Models.DTOs.Applications;

namespace SMS.Backend.Validations.Applications;

public class UpsertApplicationValidation : AbstractValidator<UpsertApplicationDto>
{
    private readonly AppDbContext _context;

    public UpsertApplicationValidation(AppDbContext context)
    {
        _context = context;

        RuleFor(a => a.StudentName).NotEmpty().WithMessage("Student name is required")
            .MaximumLength(255).WithMessage("Student name cannot exceed 255 characters");
        RuleFor(a => a.Gender).IsInEnum().WithMessage("Gender is required");
        RuleFor(a => a.GuardianName).NotEmpty().WithMessage("Guardian name is required")
            .MaximumLength(255).WithMessage("Guardian name cannot exceed 255 characters");
        RuleFor(a => a.GuardianEmail).EmailAddress().NotEmpty().WithMessage("Guardian email is required")
            .MaximumLength(255).WithMessage("Guardian email cannot exceed 255 characters");
        RuleFor(a => a.GuardianPhone).NotEmpty().WithMessage("Guardian phone is required")
            .MaximumLength(20).WithMessage("Guardian phone cannot exceed 20 characters");
        RuleFor(a => a.GuardianAddress).NotEmpty().WithMessage("Guardian address is required")
            .MaximumLength(255).WithMessage("Guardian address cannot exceed 255 characters");
        RuleFor(a => a.BirthDate).NotEmpty().WithMessage("Birth date is required");
        RuleFor(a => a.GradeId).GreaterThan(0).WithMessage("Grade id is required")
            .MustAsync(async (id, cancellation) =>
            {
                return await _context.Grades.AnyAsync(g => g.Id == id, cancellation);
            }).WithMessage("Grade id is not valid");
        RuleFor(a => a.Status).IsInEnum().WithMessage("Status is required");
        RuleFor(a => a.AcademicYearId).GreaterThan(0).WithMessage("Academic year id is required")
            .MustAsync(async (id, cancellation) =>
            {
                return await _context.AcademicYears.AnyAsync(a => a.Id == id, cancellation);
            }).WithMessage("Academic year id is not valid");
        RuleFor(a => a.InstitutionId).GreaterThan(0).WithMessage("Institution id is required")
            .MustAsync(async (id, cancellation) =>
            {
                return await _context.Institutions.AnyAsync(a => a.Id == id, cancellation);
            }).WithMessage("Institution id is not valid");
    }
}