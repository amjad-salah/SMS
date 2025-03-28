namespace SMS.Models.DTOs.InvoiceItems;

public class UpsertInvoiceItemDto
{
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public int InvoiceId { get; set; }
}