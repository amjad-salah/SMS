using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SMS.Backend.Data;
using SMS.Models.DTOs.Announcements;

namespace SMS.Backend.Validations.Announcements;

public class UpsertAnnouncementValidation : AbstractValidator<UpsertAnnouncementDto>
{
    private readonly AppDbContext _context;

    public UpsertAnnouncementValidation(AppDbContext context)
    {
        _context = context;

        RuleFor(a => a.Title).NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");
        RuleFor(a => a.Description).NotEmpty().WithMessage("Description is required.");
        RuleFor(a => a.Date).NotEmpty().WithMessage("Date is required.");
        RuleFor(a => a.ClassId).GreaterThan(0).WithMessage("Class Id is required.")
            .MustAsync(async (id, cancellation) =>
            {
                return await _context.Classes.AnyAsync(c => c.Id == id, cancellation);
            }).WithMessage("Class Id is invalid.");
    }
}