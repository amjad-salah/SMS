using System.ComponentModel.DataAnnotations.Schema;

namespace SMS.Models.Entities;

[Table("exam_results")]
public class ExamResult : BaseEntity
{
    [Column("score")] public decimal Score { get; set; }
    [Column("percentage")] public decimal Percentage { get; set; }
    [Column("approved")] public bool Approved { get; set; }
    [Column("exam_id")] public int ExamId { get; set; }
    public virtual Exam? Exam { get; set; }
    [Column("student_id")] public int StudentId { get; set; }
    [ForeignKey(nameof(StudentId))] public virtual Student? Student { get; set; }
}