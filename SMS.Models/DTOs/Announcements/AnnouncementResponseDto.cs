namespace SMS.Models.DTOs.Announcements;

public class AnnouncementResponseDto : BaseResponseDto
{
    public List<AnnouncementDto>? Announcements { get; set; }
    public AnnouncementDto? Announcement { get; set; }
}