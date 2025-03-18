using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SMS.Models.Entities;

[Table("teachers")]
[Index(nameof(Phone), IsUnique = true)]
public class Teacher : BaseEntity
{
    [Column("full_name", TypeName = "varchar(255)")]
    public string FullName { get; set; } = string.Empty;

    [Column("phone", TypeName = "varchar(20)")]
    public string Phone { get; set; } = string.Empty;

    [Column("email", TypeName = "varchar(255)")]
    public string? Email { get; set; } = string.Empty;

    [Column("join_date")] public DateTime JoinDate { get; set; }

    [Column("address", TypeName = "varchar(255)")]
    public string? Address { get; set; } = string.Empty;

    [Column("experience_years")] public decimal ExperienceYears { get; set; }
    [Column("institution_id")] public int InstitutionId { get; set; }
    [ForeignKey(nameof(InstitutionId))] public virtual Institution? Institution { get; set; }
    [Column("user_id")] public int? UserId { get; set; }
    [ForeignKey(nameof(UserId))] public virtual User? User { get; set; }
    public virtual List<Class>? Classes { get; set; }
    public virtual List<Lesson>? Lessons { get; set; }
}