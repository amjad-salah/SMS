using SMS.Models.DTOs.Students;
using SMS.Models.Entities;

namespace SMS.Models.DTOs.Invoices;

public class InvoiceDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }
    public decimal Discount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingBalance { get; set; }
    public InvoiceStatus Status { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public int StudentId { get; set; }
    public virtual StudentDto? Student { get; set; }
}