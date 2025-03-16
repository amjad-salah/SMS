using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SMS.Models.Entities;

[Table("academic_years")]
[Index(nameof(Name), IsUnique = true)]
public class AcademicYear : BaseEntity
{
    [Column("name", TypeName = "varchar(50)")]
    public string Name { get; set; } = string.Empty;

    [Column("start_date")] public DateOnly StartDate { get; set; }
    [Column("end_date")] public DateOnly EndDate { get; set; }
    [Column("is_active")] public bool IsCurrent { get; set; }
    public virtual List<Student>? Students { get; set; }
    public virtual List<Exam>? Exams { get; set; }
    public virtual List<Assignment>? Assignments { get; set; }
}