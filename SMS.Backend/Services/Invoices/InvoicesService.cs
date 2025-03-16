using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SMS.Backend.Data;
using SMS.Models.DTOs.InvoiceItems;
using SMS.Models.DTOs.Invoices;
using SMS.Models.DTOs.Payments;
using SMS.Models.Entities;

namespace SMS.Backend.Services.Invoices;

public class InvoicesService(
    AppDbContext context,
    IValidator<UpsertInvoiceDto> validator,
    IValidator<UpsertInvoiceItemDto> itemValidator,
    IValidator<UpsertPaymentDto> paymentValidator) : IInvoicesService
{
    public async Task<InvoicesResponseDto> GetInvoicesAsync()
    {
        var invoices = await context.Invoices.AsNoTracking()
            .Include(i => i.Student)
            .OrderByDescending(i => i.CreatedAt)
            .ProjectToType<InvoiceDto>()
            .ToListAsync();

        return new InvoicesResponseDto() { Success = true, Invoices = invoices };
    }

    public async Task<InvoicesResponseDto> GetInvoiceByCodeAsync(string code)
    {
        var invoice = await context.Invoices.AsNoTracking()
            .Include(i => i.Student)
            .Include(i => i.Items)
            .Include(i => i.Payments)
            .FirstOrDefaultAsync(i => i.InvoiceNo == code);

        return invoice == null
            ? new InvoicesResponseDto() { Success = false, Message = "Invoice not found" }
            : new InvoicesResponseDto() { Success = true, Invoice = invoice.Adapt<InvoiceDetailsDto>() };
    }

    public async Task<InvoicesResponseDto> GetInvoiceSByCurrentYearAsync()
    {
        var invoices = await context.Invoices.AsNoTracking()
            // .Include(i => i.)
            .Include(i => i.Student)
            .OrderByDescending(i => i.CreatedAt)
            .ProjectToType<InvoiceDto>()
            .ToListAsync();

        return new InvoicesResponseDto() { Success = true, Invoices = invoices };
    }

    public async Task<InvoicesResponseDto> AddInvoiceAsync(UpsertInvoiceDto invoice)
    {
        var validationResult = await validator.ValidateAsync(invoice);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));

            Log.Error(error);

            return new InvoicesResponseDto() { Success = false, Message = error };
        }

        var currentYear = await context.AcademicYears.AsNoTracking()
            .FirstOrDefaultAsync(a => a.IsCurrent);

        var newInvoice = invoice.Adapt<Invoice>();

        var invoiceNumber = context.Invoices.Any() ? context.Invoices.Max(i => i.Id) + 1 : 1;

        newInvoice.InvoiceNo = $"INV0{invoiceNumber}";
        newInvoice.Status = InvoiceStatus.Pending;
        newInvoice.AcademicYearId = currentYear!.Id;

        await context.Invoices.AddAsync(newInvoice);
        await context.SaveChangesAsync();

        return new InvoicesResponseDto()
        {
            Success = true,
            Message = "Invoice added successfully!",
            Invoice = newInvoice.Adapt<InvoiceDetailsDto>()
        };
    }

    public async Task<InvoicesResponseDto> DeleteInvoiceAsync(int id)
    {
        var invoice = await context.Invoices.FindAsync(id);

        if (invoice == null)
            return new InvoicesResponseDto() { Success = true, Message = "Invoice not found" };

        context.Invoices.Remove(invoice);
        await context.SaveChangesAsync();

        return new InvoicesResponseDto() { Success = true, Message = "Invoice deleted successfully!" };
    }

    public async Task<InvoiceItemsResponseDto> AddInvoiceItemAsync(UpsertInvoiceItemDto upsertInvoiceItem)
    {
        var validationResult = await itemValidator.ValidateAsync(upsertInvoiceItem);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));

            Log.Error(error);

            return new InvoiceItemsResponseDto() { Success = false, Message = error };
        }

        var newInvoiceItem = upsertInvoiceItem.Adapt<InvoiceItem>();

        newInvoiceItem.Total = newInvoiceItem.UnitPrice * upsertInvoiceItem.Quantity;

        var invoice = await context.Invoices.FindAsync(upsertInvoiceItem.InvoiceId);

        if (invoice != null)
        {
            invoice.SubTotal += newInvoiceItem.Total;
            var taxAmount = invoice.SubTotal * invoice.Tax / 100;
            var discountAmount = invoice.SubTotal * invoice.Discount / 100;
            invoice.Total = invoice.SubTotal + taxAmount - discountAmount;
            invoice.RemainingBalance = invoice.Total - invoice.PaidAmount;
        }

        await context.InvoiceItems.AddAsync(newInvoiceItem);
        await context.SaveChangesAsync();

        return new InvoiceItemsResponseDto() { Success = true, Message = "Invoice item added successfully!" };
    }

    public async Task<InvoiceItemsResponseDto> DeleteInvoiceItemAsync(int invoiceItemId)
    {
        var invoiceItem = await context.InvoiceItems.FindAsync(invoiceItemId);

        if (invoiceItem == null)
            return new InvoiceItemsResponseDto() { Success = false, Message = "Invoice item not found" };

        var invoice = await context.Invoices.FindAsync(invoiceItem.InvoiceId);

        if (invoice != null)
        {
            invoice.SubTotal -= invoiceItem.Total;
            var taxAmount = invoice.SubTotal * invoice.Tax / 100;
            var discountAmount = invoice.SubTotal * invoice.Discount / 100;
            invoice.Total = invoice.SubTotal + taxAmount - discountAmount;
            invoice.RemainingBalance = invoice.Total - invoice.PaidAmount;
        }

        context.InvoiceItems.Remove(invoiceItem);
        await context.SaveChangesAsync();

        return new InvoiceItemsResponseDto() { Success = true, Message = "Invoice item deleted successfully!" };
    }

    public async Task<PaymentResponseDto> AddPaymentAsync(UpsertPaymentDto payment)
    {
        var validationResult = await paymentValidator.ValidateAsync(payment);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));

            Log.Error(error);

            return new PaymentResponseDto() { Success = false, Message = error };
        }

        var newPayment = payment.Adapt<Payment>();

        var invoice = await context.Invoices.FindAsync(payment.InvoiceId);

        if (invoice != null)
        {
            invoice.PaidAmount += newPayment.Amount;
            invoice.RemainingBalance = invoice.Total - invoice.PaidAmount;
        }

        await context.Payments.AddAsync(newPayment);
        await context.SaveChangesAsync();

        return new PaymentResponseDto() { Success = true, Message = "Payment added successfully!" };
    }

    public async Task<PaymentResponseDto> DeletePaymentAsync(int paymentId)
    {
        var payment = await context.Payments.FindAsync(paymentId);

        if (payment == null)
            return new PaymentResponseDto() { Success = false, Message = "Payment not found" };

        var invoice = await context.Invoices.FindAsync(payment.InvoiceId);

        if (invoice != null)
        {
            invoice.PaidAmount -= payment.Amount;
            invoice.RemainingBalance = invoice.Total - invoice.PaidAmount;
        }

        context.Payments.Remove(payment);
        await context.SaveChangesAsync();

        return new PaymentResponseDto() { Success = true, Message = "Payment deleted successfully!" };
    }
}