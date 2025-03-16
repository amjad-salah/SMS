namespace SMS.Models.DTOs.Invoices;

public class UpsertInvoiceDto
{
    public DateOnly Date { get; set; }
    public decimal Tax { get; set; }
    public decimal Discount { get; set; }
    public int StudentId { get; set; }
}