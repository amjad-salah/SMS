using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SMS.Models.Entities;

[Table("users")]
[Index(nameof(Email), IsUnique = true)]
public class User : BaseEntity
{
    [Column("full_name", TypeName = "varchar(255")]
    public string FullName { get; set; } = string.Empty;

    [Column("email", TypeName = "varchar(255)")]
    public string Email { get; set; } = string.Empty;

    [Column("password", TypeName = "varchar(255)")]
    public string Password { get; set; } = string.Empty;

    [Column("role")] public UserRole Role { get; set; }
    [Column("institution_id")] public int? InstitutionId { get; set; }
    [ForeignKey(nameof(InstitutionId))] public virtual Institution? Institution { get; set; }
    public Teacher? Teacher { get; set; }
    public Student? Student { get; set; }
    public Parent? Parent { get; set; }
}

public enum UserRole
{
    Admin,
    Teacher,
    Parent,
    Student,
    Support,
    Registrar,
    Accountant
}