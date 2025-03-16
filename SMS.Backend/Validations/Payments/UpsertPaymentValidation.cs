using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SMS.Backend.Data;
using SMS.Models.DTOs.Payments;

namespace SMS.Backend.Validations.Payments;

public class UpsertPaymentValidation : AbstractValidator<UpsertPaymentDto>
{
    private readonly AppDbContext _context;

    public UpsertPaymentValidation(AppDbContext context)
    {
        _context = context;

        RuleFor(p => p.Amount).GreaterThan(0).WithMessage("Amount must be greater than 0");
        RuleFor(p => p.InvoiceId).GreaterThan(0).WithMessage("Invoice Id is required")
            .MustAsync(async (id, cancellation) =>
            {
                return await _context.Invoices.AnyAsync(g => g.Id == id, cancellation);
            }).WithMessage("Invoice id is not valid");
    }
}