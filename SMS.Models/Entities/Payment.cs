using System.ComponentModel.DataAnnotations.Schema;

namespace SMS.Models.Entities;

[Table("payments")]
public class Payment : BaseEntity
{
    [Column("amount")] public decimal Amount { get; set; }
    [Column("date")] public DateOnly Date { get; set; }

    [Column("transaction_id", TypeName = "varchar(18)")]
    public string? TransactionId { get; set; }

    [Column("invoice_id")] public int InvoiceId { get; set; }
    [ForeignKey(nameof(InvoiceId))] public virtual Invoice? Invoice { get; set; }
}