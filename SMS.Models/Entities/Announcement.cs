using System.ComponentModel.DataAnnotations.Schema;

namespace SMS.Models.Entities;

[Table("announcements")]
public class Announcement : BaseEntity
{
    [Column("title", TypeName = "varchar(200)")]
    public string Title { get; set; } = string.Empty;

    [Column("description", TypeName = "text")]
    public string Description { get; set; } = string.Empty;

    [Column("date")] public DateTime Date { get; set; }
    [Column("class_id")] public int? ClassId { get; set; }
    [ForeignKey(nameof(ClassId))] public virtual Class? Class { get; set; }
}