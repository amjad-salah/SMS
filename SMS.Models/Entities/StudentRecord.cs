using System.ComponentModel.DataAnnotations.Schema;

namespace SMS.Models.Entities;

[Table("student_records")]
public class StudentRecord : BaseEntity
{
    [Column("student_id")] public int StudentId { get; set; }
    [ForeignKey(nameof(StudentId))] public virtual Student? Student { get; set; }
    [Column("record_id")] public int GradeId { get; set; }
    [ForeignKey(nameof(GradeId))] public virtual Grade? Grade { get; set; }
    [Column("academic_year_id")] public int AcademicYearId { get; set; }
    [ForeignKey(nameof(AcademicYearId))] public virtual AcademicYear? AcademicYear { get; set; }
    [Column("result")] public Result? Result { get; set; }

    [Column("remarks", TypeName = "varchar(10)")]
    public string? Remarks { get; set; }

    [Column("total_remark", TypeName = "varchar(10)")]
    public string? TotalRemark { get; set; }

    [Column("percentage", TypeName = "varchar(10)")]
    public string? Percentage { get; set; }

    [Column("notes", TypeName = "varchar(500)")]
    public string? Notes { get; set; }
}

public enum Result
{
    Success,
    Failed
}