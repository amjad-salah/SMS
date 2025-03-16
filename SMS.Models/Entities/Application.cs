using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SMS.Models.Entities;

[Table("applications")]
[Index(nameof(ApplicationNo), IsUnique = true)]
public class Application : BaseEntity
{
    [Column("student_name", TypeName = "varchar(255)")]
    public string StudentName { get; set; } = string.Empty;

    [Column("guardian_name", TypeName = "varchar(255)")]
    public string GuardianName { get; set; } = string.Empty;

    [Column("guardian_phone", TypeName = "varchar(20)")]
    public string GuardianPhone { get; set; } = string.Empty;

    [Column("guardian_email", TypeName = "varchar(255)")]
    public string? GuardianEmail { get; set; } = string.Empty;

    [Column("guardian_address", TypeName = "varchar(255)")]
    public string GuardianAddress { get; set; } = string.Empty;

    [Column("grade_id")] public int GradeId { get; set; }
    [ForeignKey(nameof(GradeId))] public virtual Grade? Grade { get; set; }
    [Column("birth_date")] public DateOnly BirthDate { get; set; }
    [Column("gender")] public Gender Gender { get; set; }
    [Column("application_status")] public ApplicationStatus Status { get; set; }

    [Column("application_no", TypeName = "varchar(50)")]
    public string ApplicationNo { get; set; } = string.Empty;

    [Column("academic_year_id")] public int AcademicYearId { get; set; }
    [ForeignKey(nameof(AcademicYearId))] public virtual AcademicYear? AcademicYear { get; set; }
    [Column("institution_id")] public int InstitutionId { get; set; }
    [ForeignKey(nameof(InstitutionId))] public virtual Institution? Institution { get; set; }
}

public enum ApplicationStatus
{
    Pending,
    Approved,
    Rejected
}