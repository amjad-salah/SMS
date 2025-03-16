using SMS.Models.DTOs.InvoiceItems;
using SMS.Models.DTOs.Invoices;
using SMS.Models.DTOs.Payments;

namespace SMS.Backend.Services.Invoices;

public interface IInvoicesService
{
    Task<InvoicesResponseDto> GetInvoicesAsync();
    Task<InvoicesResponseDto> GetInvoiceByCodeAsync(string code);
    Task<InvoicesResponseDto> GetInvoiceSByCurrentYearAsync();
    Task<InvoicesResponseDto> AddInvoiceAsync(UpsertInvoiceDto invoice);
    Task<InvoicesResponseDto> DeleteInvoiceAsync(int id);
    Task<InvoiceItemsResponseDto> AddInvoiceItemAsync(UpsertInvoiceItemDto upsertInvoiceItem);
    Task<InvoiceItemsResponseDto> DeleteInvoiceItemAsync(int invoiceItemId);
    Task<PaymentResponseDto> AddPaymentAsync(UpsertPaymentDto payment);
    Task<PaymentResponseDto> DeletePaymentAsync(int paymentId);
}