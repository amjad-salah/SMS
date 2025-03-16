namespace SMS.Models.DTOs.Announcements;

public class UpsertAnnouncementDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public int? ClassId { get; set; }
}