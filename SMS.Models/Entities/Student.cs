using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SMS.Models.Entities;

[Table("students")]
[Index(nameof(StudentNo), IsUnique = true)]
public class Student : BaseEntity
{
    [Column("full_name", TypeName = "varchar(255)")]
    public string FullName { get; set; } = string.Empty;

    [Column("grade_id")] public int GradeId { get; set; }
    [ForeignKey(nameof(GradeId))] public virtual Grade? Grade { get; set; }
    [Column("class_id")] public int ClassId { get; set; }
    [ForeignKey(nameof(ClassId))] public virtual Class? Class { get; set; }

    [Column("student_no", TypeName = "varchar(50)")]
    public string StudentNo { get; set; } = string.Empty;

    [Column("birth_date")] public DateTime BirthDate { get; set; }
    [Column("parent_id")] public int ParentId { get; set; }
    [ForeignKey(nameof(ParentId))] public virtual Parent? Parent { get; set; }
    [Column("admission_date")] public DateTime AdmissionDate { get; set; }

    [Column("medical_info", TypeName = "varchar(500)")]
    public string? MedicalInfo { get; set; }

    [Column("gender")] public Gender Gender { get; set; }
    [Column("user_id")] public int? UserId { get; set; }
    [ForeignKey(nameof(UserId))] public virtual User? User { get; set; }
    [Column("status")] public StudentStatus Status { get; set; }
    [Column("academic_year_id")] public int AcademicYearId { get; set; }
    [ForeignKey(nameof(AcademicYearId))] public virtual AcademicYear? AcademicYear { get; set; }
    [Column("institution_id")] public int InstitutionId { get; set; }
    [ForeignKey(nameof(InstitutionId))] public virtual Institution? Institution { get; set; }
    public virtual List<StudentRecord>? Records { get; set; }
    public virtual List<ExamResult>? ExamResults { get; set; }
    public virtual List<Attendance>? Attendances { get; set; }
    public virtual List<Invoice>? Invoices { get; set; }
}

public enum StudentStatus
{
    Active,
    Inactive,
    Graduated,
    Left
}

public enum Gender
{
    Male,
    Female
}