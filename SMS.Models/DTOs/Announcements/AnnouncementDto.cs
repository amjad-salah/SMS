using SMS.Models.DTOs.Classes;

namespace SMS.Models.DTOs.Announcements;

public class AnnouncementDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public int? ClassId { get; set; }
    public virtual ClassDto? Class { get; set; }
}