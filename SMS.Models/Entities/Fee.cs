using System.ComponentModel.DataAnnotations.Schema;

namespace SMS.Models.Entities;

[Table("fees")]
public class Fee : BaseEntity
{
    [Column("amount", TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    [Column("type")] public FeeType Type { get; set; }

    [Column("name", TypeName = "varchar(255)")]
    public string Name { get; set; } = string.Empty;
}

public enum FeeType
{
    Register,
    Study,
    Transport,
    Other
}