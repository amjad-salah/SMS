using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SMS.Backend.Data;
using SMS.Models.DTOs.InvoiceItems;

namespace SMS.Backend.Validations.InvoiceItems;

public class UpsertInvoiceItemValidation : AbstractValidator<UpsertInvoiceItemDto>
{
    private readonly AppDbContext _context;

    public UpsertInvoiceItemValidation(AppDbContext context)
    {
        _context = context;

        RuleFor(i => i.Description).NotEmpty().WithMessage("Description is required.")
            .MaximumLength(100).WithMessage("Description must not exceed 100 characters.");
        RuleFor(i => i.InvoiceId).GreaterThan(0).WithMessage("Invoice Id is invalid.")
            .MustAsync(async (id, cancellation) =>
            {
                return await _context.Invoices.AnyAsync(g => g.Id == id, cancellation);
            }).WithMessage("Invoice id is not valid");
        RuleFor(i => i.Quantity).GreaterThan(0).WithMessage("Quantity is invalid.");
        RuleFor(i => i.UnitPrice).GreaterThan(0).WithMessage("Unit Price is invalid.");
    }
}