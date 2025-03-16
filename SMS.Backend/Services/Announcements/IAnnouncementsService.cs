using SMS.Models.DTOs.Announcements;

namespace SMS.Backend.Services.Announcements;

public interface IAnnouncementsService
{
    Task<AnnouncementResponseDto> GetAnnouncementsAsync();
    Task<AnnouncementResponseDto> GetAnnouncementByIdAsync(int id);
    Task<AnnouncementResponseDto> AddAnnouncementAsync(UpsertAnnouncementDto announcement, int userId);
    Task<AnnouncementResponseDto> UpdateAnnouncementAsync(int id, UpsertAnnouncementDto announcement, int userId);
    Task<AnnouncementResponseDto> DeleteAnnouncementAsync(int id, int userId);
}