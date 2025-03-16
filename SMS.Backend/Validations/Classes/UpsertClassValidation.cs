using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SMS.Backend.Data;
using SMS.Models.DTOs.Classes;

namespace SMS.Backend.Validations.Classes;

public class UpsertClassValidation : AbstractValidator<UpsertClassDto>
{
    private readonly AppDbContext _context;

    public UpsertClassValidation(AppDbContext context)
    {
        _context = context;

        RuleFor(c => c.Name).NotEmpty().WithMessage("Name is required")
            .MaximumLength(255).WithMessage("Name must not exceed 255 characters");
        RuleFor(c => c.GradeId).GreaterThan(0).WithMessage("Grade Id is required")
            .MustAsync(async (id, cancellation) =>
            {
                return await _context.Grades.AnyAsync(g => g.Id == id, cancellation);
            }).WithMessage("Grade id is not valid");
        RuleFor(c => c.InstitutionId).GreaterThan(0).WithMessage("Institution Id is required")
            .MustAsync(async (id, cancellation) =>
            {
                return await _context.Institutions.AnyAsync(g => g.Id == id, cancellation);
            }).WithMessage("Institution id is not valid");
        RuleFor(c => c.TeacherId).GreaterThan(0).WithMessage("Teacher Id is required")
            .MustAsync(async (id, cancellation) =>
            {
                return await _context.Teachers.AnyAsync(g => g.Id == id, cancellation);
            }).WithMessage("Teacher id is not valid");
        RuleFor(c => c.Capacity).GreaterThan(0).WithMessage("Capacity is required");
    }
}