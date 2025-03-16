using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SMS.Models.Entities;

[Table("parents")]
[Index(nameof(Phone), IsUnique = true)]
public class Parent : BaseEntity
{
    [Column("full_name", TypeName = "varchar(100)")]
    public string FullName { get; set; } = string.Empty;

    [Column("email", TypeName = "varchar(100)")]
    public string Email { get; set; } = string.Empty;

    [Column("phone", TypeName = "varchar(20)")]
    public string Phone { get; set; } = string.Empty;

    [Column("address", TypeName = "varchar(100)")]
    public string Address { get; set; } = string.Empty;

    public virtual List<Student>? Students { get; set; }
    [Column("user_id")] public int? UserId { get; set; }
    [ForeignKey(nameof(UserId))] public virtual User? User { get; set; }
}