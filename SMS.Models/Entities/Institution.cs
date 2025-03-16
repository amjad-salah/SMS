using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SMS.Models.Entities;

[Table("institutions")]
[Index(nameof(Name), IsUnique = true)]
public class Institution : BaseEntity
{
    [Column("name", TypeName = "varchar(255)")]
    public string Name { get; set; } = string.Empty;

    [Column("address", TypeName = "varchar(255)")]
    public string Address { get; set; } = string.Empty;

    [Column("phone", TypeName = "varchar(20)")]
    public string Phone { get; set; } = string.Empty;

    [Column("email", TypeName = "varchar(255)")]
    public string Email { get; set; } = string.Empty;

    [Column("date_established")] public DateOnly DateEstablished { get; set; }
    [Column("institution_type")] public InstitutionType InstitutionType { get; set; }

    public virtual List<User>? Users { get; set; }
    public virtual List<Teacher>? Teachers { get; set; }
    public virtual List<Student>? Students { get; set; }
    public virtual List<Class>? Classes { get; set; }
    public virtual List<Application>? Applications { get; set; }
}

public enum InstitutionType
{
    Primary,
    Middle,
    Secondary,
    Combined
}