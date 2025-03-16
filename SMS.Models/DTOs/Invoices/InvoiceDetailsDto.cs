using SMS.Models.DTOs.InvoiceItems;
using SMS.Models.DTOs.Payments;

namespace SMS.Models.DTOs.Invoices;

public class InvoiceDetailsDto : InvoiceDto
{
    public virtual List<InvoiceItemDto>? Items { get; set; }
    public virtual List<PaymentDto>? Payments { get; set; }
}