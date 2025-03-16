using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SMS.Models.Entities;

[Table("classes")]
[Index(nameof(Name), IsUnique = true)]
public class Class : BaseEntity
{
    [Column("name", TypeName = "varchar(255)")]
    public string Name { get; set; } = string.Empty;

    [Column("grade_id")] public int GradeId { get; set; }
    [ForeignKey(nameof(GradeId))] public virtual Grade? Grade { get; set; }
    [Column("institution_id")] public int InstitutionId { get; set; }
    [ForeignKey(nameof(InstitutionId))] public virtual Institution? Institution { get; set; }
    [Column("teacher_id")] public int TeacherId { get; set; }
    [ForeignKey(nameof(TeacherId))] public virtual Teacher? Teacher { get; set; }
    [Column("capacity")] public int Capacity { get; set; }
    public virtual List<Lesson>? Lessons { get; set; }
    public virtual List<Student>? Students { get; set; }
    public virtual List<Assignment>? Assignments { get; set; }
    public virtual List<Announcement>? Announcements { get; set; }
    public virtual List<Attendance>? Attendances { get; set; }
}