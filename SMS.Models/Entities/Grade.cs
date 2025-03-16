using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SMS.Models.Entities;

[Table("grades")]
[Index(nameof(Name), IsUnique = true)]
public class Grade : BaseEntity
{
    [Column("name", TypeName = "varchar(255)")]
    public string Name { get; set; } = string.Empty;

    public virtual List<Class>? Classes { get; set; }
    public virtual List<Student>? Students { get; set; }
    public virtual List<Exam>? Exams { get; set; }
    public virtual List<Subject>? Subjects { get; set; }
}