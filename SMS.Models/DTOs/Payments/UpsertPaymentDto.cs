namespace SMS.Models.DTOs.Payments;

public class UpsertPaymentDto
{
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? TransactionId { get; set; }
    public int InvoiceId { get; set; }
}