using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SMS.Backend.Data;
using SMS.Models.DTOs.Invoices;

namespace SMS.Backend.Validations.Invoices;

public class UpsertInvoiceValidation : AbstractValidator<UpsertInvoiceDto>
{
    private readonly AppDbContext _context;

    public UpsertInvoiceValidation(AppDbContext context)
    {
        _context = context;

        RuleFor(i => i.Date).NotEmpty().WithMessage("Invoice Date is required.");
        RuleFor(i => i.StudentId).GreaterThan(0).WithMessage("Student Id is invalid.")
            .MustAsync(async (id, cancellation) =>
            {
                return await _context.Students.AnyAsync(g => g.Id == id, cancellation);
            }).WithMessage("Student id is not valid");
        RuleFor(i => i.Discount).NotEmpty().WithMessage("Invoice Discount is required.");
        RuleFor(i => i.Tax).NotEmpty().WithMessage("Invoice Tax is required.");
    }
}