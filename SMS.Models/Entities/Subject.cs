using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SMS.Models.Entities;

[Table("subjects")]
[Index(nameof(Name), IsUnique = true)]
public class Subject : BaseEntity
{
    [Column("name", TypeName = "varchar(100)")]
    public string Name { get; set; } = string.Empty;

    [Column("grade_id")] public int GradeId { get; set; }
    [ForeignKey(nameof(GradeId))] public virtual Grade? Grade { get; set; }
    public virtual List<Lesson>? Lessons { get; set; }
}