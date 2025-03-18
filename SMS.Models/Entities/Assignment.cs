using System.ComponentModel.DataAnnotations.Schema;

namespace SMS.Models.Entities;

[Table("assignments")]
public class Assignment : BaseEntity
{
    [Column("title", TypeName = "varchar(50)")]
    public string Title { get; set; } = string.Empty;

    [Column("start_date")] public DateTime StartDate { get; set; }
    [Column("end_date")] public DateTime EndDate { get; set; }
    [Column("class_id")] public int ClassId { get; set; }
    [ForeignKey(nameof(ClassId))] public virtual Class? Class { get; set; }
    [Column("academic_year_id")] public int AcademicYearId { get; set; }
    [ForeignKey(nameof(AcademicYearId))] public virtual AcademicYear? AcademicYear { get; set; }
}