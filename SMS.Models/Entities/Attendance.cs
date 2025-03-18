using System.ComponentModel.DataAnnotations.Schema;

namespace SMS.Models.Entities;

[Table("attendances")]
public class Attendance : BaseEntity
{
    [Column("date")] public DateTime Date { get; set; }
    [Column("present")] public bool Present { get; set; }
    [Column("class_id")] public int ClassId { get; set; }
    public virtual Class? Class { get; set; }
    [Column("student_id")] public int StudentId { get; set; }
    public virtual Student? Student { get; set; }
}