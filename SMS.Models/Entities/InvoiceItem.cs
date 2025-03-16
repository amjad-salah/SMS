using System.ComponentModel.DataAnnotations.Schema;

namespace SMS.Models.Entities;

[Table("invoice_items")]
public class InvoiceItem : BaseEntity
{
    [Column("description", TypeName = "varchar(100)")]
    public string Description { get; set; } = string.Empty;

    [Column("quantity")] public int Quantity { get; set; }

    [Column("unit_price", TypeName = "decimal(18, 2)")]
    public decimal UnitPrice { get; set; }

    [Column("total", TypeName = "decimal(18, 2)")]
    public decimal Total { get; set; }

    [Column("invoice_id")] public int InvoiceId { get; set; }
    [ForeignKey(nameof(InvoiceId))] public virtual Invoice? Invoice { get; set; }
}