using SMS.Models.DTOs.Invoices;

namespace SMS.Models.DTOs.InvoiceItems;

public class InvoiceItemDto
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Total { get; set; }
    public int InvoiceId { get; set; }
    public virtual InvoiceDto? Invoice { get; set; }
}