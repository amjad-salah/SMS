using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SMS.Backend.Data;
using SMS.Models.DTOs.Teachers;

namespace SMS.Backend.Validations.Teachers;

public class UpsertTeacherValidation : AbstractValidator<UpsertTeacherDto>
{
    private readonly AppDbContext _context;

    public UpsertTeacherValidation(AppDbContext context)
    {
        _context = context;

        RuleFor(t => t.FullName).NotEmpty().WithMessage("Full name is required")
            .MaximumLength(255).WithMessage("Full name cannot exceed 255 characters");
        RuleFor(t => t.Email).EmailAddress().WithMessage("Invalid email address")
            .MaximumLength(255).WithMessage("Email cannot exceed 255 characters");
        RuleFor(t => t.Address).NotEmpty().WithMessage("Address is required")
            .MaximumLength(255).WithMessage("Phone number cannot exceed 255 characters");
        RuleFor(t => t.Phone).NotEmpty().WithMessage("Phone number is required")
            .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters");
        RuleFor(t => t.InstitutionId).GreaterThan(0).WithMessage("Institution Id is required")
            .MustAsync(async (id, cancellation) =>
            {
                return await _context.Institutions.AnyAsync(g => g.Id == id, cancellation);
            }).WithMessage("Institution id is not valid");
        RuleFor(t => t.JoinDate).NotEmpty().WithMessage("Join date is required");
        RuleFor(t => t.ExperienceYears).NotEmpty().WithMessage("Experience years is required");
    }
}