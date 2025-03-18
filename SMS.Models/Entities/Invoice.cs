using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SMS.Models.Entities;

[Table("invoices")]
[Index(nameof(InvoiceNo), IsUnique = true)]
public class Invoice : BaseEntity
{
    [Column("date")] public DateTime Date { get; set; }

    [Column("sub_total", TypeName = "decimal(18, 2)")]
    public decimal SubTotal { get; set; }

    [Column("tax", TypeName = "decimal(3, 2)")]
    public decimal Tax { get; set; }

    [Column("total", TypeName = "decimal(18, 2)")]
    public decimal Total { get; set; }

    [Column("discount", TypeName = "decimal(3, 2)")]
    public decimal Discount { get; set; }

    [Column("paid_amount", TypeName = "decimal(18, 2)")]
    public decimal PaidAmount { get; set; }

    [Column("remaining_balance", TypeName = "decimal(18, 2)")]
    public decimal RemainingBalance { get; set; }

    [Column("status")] public InvoiceStatus Status { get; set; }

    [Column("invoice_number", TypeName = "varchar(50)")]
    public string InvoiceNo { get; set; } = string.Empty;

    [Column("student_id")] public int StudentId { get; set; }
    [ForeignKey(nameof(StudentId))] public virtual Student? Student { get; set; }
    [Column("academic_year_id")] public int AcademicYearId { get; set; }
    [ForeignKey(nameof(AcademicYearId))] public virtual AcademicYear? AcademicYear { get; set; }
    public virtual List<InvoiceItem>? Items { get; set; }
    public virtual List<Payment>? Payments { get; set; }
}

public enum InvoiceStatus
{
    Pending,
    Paid,
    Overdue
}