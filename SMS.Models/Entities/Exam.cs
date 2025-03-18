using System.ComponentModel.DataAnnotations.Schema;

namespace SMS.Models.Entities;

[Table("exams")]
public class Exam : BaseEntity
{
    [Column("title", TypeName = "varchar(50)")]
    public string Title { get; set; } = string.Empty;

    [Column("exam_date")] public DateTime ExamDate { get; set; }
    [Column("start_time")] public TimeOnly StartTime { get; set; }
    [Column("end_time")] public TimeOnly EndTime { get; set; }
    [Column("exam_type_id")] public int ExamTypeId { get; set; }
    [ForeignKey(nameof(ExamTypeId))] public virtual ExamType? ExamType { get; set; }
    [Column("max_mark")] public decimal MaxMark { get; set; }
    [Column("min_mark")] public decimal MinMark { get; set; }
    [Column("grade_id")] public int GradeId { get; set; }
    [ForeignKey(nameof(GradeId))] public virtual Grade? Grade { get; set; }
    [Column("academic_year_id")] public int AcademicYearId { get; set; }
    [ForeignKey(nameof(AcademicYearId))] public virtual AcademicYear? AcademicYear { get; set; }
    public virtual List<ExamResult>? Results { get; set; }
}