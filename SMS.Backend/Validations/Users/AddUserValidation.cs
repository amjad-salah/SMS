using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SMS.Backend.Data;
using SMS.Models.DTOs.Users;

namespace SMS.Backend.Validations.Users;

public class AddUserValidation : AbstractValidator<AddUserDto>
{
    private readonly AppDbContext _context;

    public AddUserValidation(AppDbContext context)
    {
        _context = context;

        RuleFor(u => u.Email).NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is required");
        RuleFor(u => u.Password).NotEmpty().WithMessage("Password is required");
        RuleFor(u => u.Role).IsInEnum().WithMessage("Role is required");
        RuleFor(u => u.FullName).NotEmpty().WithMessage("Full name is required");
        RuleFor(u => u.InstitutionId).NotEmpty().WithMessage("Institution is required")
            .MustAsync(async (id, cancellation) =>
            {
                return await _context.Institutions.AnyAsync(g => g.Id == id, cancellation);
            }).WithMessage("Institution id is not valid");
    }
}