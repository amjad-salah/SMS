using System.ComponentModel.DataAnnotations.Schema;

namespace SMS.Models.Entities;

[Table("lessons")]
public class Lesson : BaseEntity
{
    [Column("day")] public DayOfWeek Day { get; set; }
    [Column("start_time")] public TimeOnly StartTime { get; set; }
    [Column("end_time")] public TimeOnly EndTime { get; set; }
    [Column("subject_id")] public int SubjectId { get; set; }
    [ForeignKey(nameof(SubjectId))] public virtual Subject? Subject { get; set; }
    [Column("class_id")] public int ClassId { get; set; }
    [ForeignKey(nameof(ClassId))] public virtual Class? Class { get; set; }
    [Column("teacher_id")] public int TeacherId { get; set; }
    [ForeignKey(nameof(TeacherId))] public virtual Teacher? Teacher { get; set; }
}