using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SMS.Models.Entities;

[Table("exam_types")]
[Index(nameof(Name), IsUnique = true)]
public class ExamType : BaseEntity
{
    [Column("name", TypeName = "varchar(20)")]
    public string Name { get; set; } = string.Empty;

    public virtual List<Exam>? Exams { get; set; }
}