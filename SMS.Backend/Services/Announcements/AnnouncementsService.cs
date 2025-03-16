using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SMS.Backend.Data;
using SMS.Models.DTOs.Announcements;
using SMS.Models.Entities;

namespace SMS.Backend.Services.Announcements;

public class AnnouncementsService(
    AppDbContext context,
    IValidator<UpsertAnnouncementDto> validator) : IAnnouncementsService
{
    public async Task<AnnouncementResponseDto> GetAnnouncementsAsync()
    {
        var announcements = await context.Announcements.AsNoTracking()
            .Include(a => a.Class)
            .OrderByDescending(a => a.CreatedAt)
            .ProjectToType<AnnouncementDto>()
            .ToListAsync();

        return new AnnouncementResponseDto() { Success = true, Announcements = announcements };
    }

    public async Task<AnnouncementResponseDto> GetAnnouncementByIdAsync(int id)
    {
        var announcement = await context.Announcements.AsNoTracking()
            .Include(a => a.Class)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (announcement != null)
            return new AnnouncementResponseDto()
                { Success = true, Announcement = announcement.Adapt<AnnouncementDto>() };

        Log.Error("Announcement with id {Id} not found", id);
        return new AnnouncementResponseDto() { Success = false, Message = "Announcement not found" };
    }

    public async Task<AnnouncementResponseDto> AddAnnouncementAsync(UpsertAnnouncementDto announcement, int userId)
    {
        var validationResult = await validator.ValidateAsync(announcement);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));

            Log.Error(error);

            return new AnnouncementResponseDto() { Success = false, Message = error };
        }

        var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);

        // ReSharper disable once NullableWarningSuppressionIsUsed
        if (user!.Role == UserRole.Teacher)
        {
            var teacher = await context.Teachers.AsNoTracking().FirstOrDefaultAsync(t => t.UserId == userId);

            var anClass = await context.Classes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == announcement.ClassId);

            // ReSharper disable once NullableWarningSuppressionIsUsed
            if (teacher!.Id != anClass!.TeacherId)
            {
                Log.Error("Teacher is not class's teacher");
                return new AnnouncementResponseDto() { Success = false, Message = "Teacher is not class's teacher" };
            }
        }

        var newAnnouncement = announcement.Adapt<Announcement>();

        await context.Announcements.AddAsync(newAnnouncement);
        await context.SaveChangesAsync();

        return new AnnouncementResponseDto()
        {
            Success = true,
            Announcement = newAnnouncement.Adapt<AnnouncementDto>(),
            Message = "Announcement added successfully!"
        };
    }

    public async Task<AnnouncementResponseDto> UpdateAnnouncementAsync(int id, UpsertAnnouncementDto announcement,
        int userId)
    {
        var validationResult = await validator.ValidateAsync(announcement);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));

            Log.Error(error);

            return new AnnouncementResponseDto() { Success = false, Message = error };
        }

        var existingAnnouncement = await context.Announcements.FindAsync(id);

        if (existingAnnouncement == null)
        {
            Log.Error("Announcement with id {Id} not found", id);
            return new AnnouncementResponseDto() { Success = false, Message = "Announcement not found" };
        }

        var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);

        // ReSharper disable once NullableWarningSuppressionIsUsed
        if (user!.Role == UserRole.Teacher)
        {
            var teacher = await context.Teachers.AsNoTracking().FirstOrDefaultAsync(t => t.UserId == userId);

            var anClass = await context.Classes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == announcement.ClassId);

            // ReSharper disable once NullableWarningSuppressionIsUsed
            if (teacher!.Id != anClass!.TeacherId)
            {
                Log.Error("Teacher is not class's teacher");
                return new AnnouncementResponseDto() { Success = false, Message = "Teacher is not class's teacher" };
            }
        }

        existingAnnouncement.ClassId = announcement.ClassId;
        existingAnnouncement.Date = announcement.Date;
        existingAnnouncement.Description = announcement.Description;
        existingAnnouncement.Title = announcement.Title;
        await context.SaveChangesAsync();

        return new AnnouncementResponseDto() { Success = true, Message = "Announcement updated successfully!" };
    }

    public async Task<AnnouncementResponseDto> DeleteAnnouncementAsync(int id, int userId)
    {
        var existingAnnouncement = await context.Announcements.FindAsync(id);

        if (existingAnnouncement == null)
        {
            Log.Error("Announcement with id {Id} not found", id);
            return new AnnouncementResponseDto() { Success = false, Message = "Announcement not found" };
        }

        var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);

        // ReSharper disable once NullableWarningSuppressionIsUsed
        if (user!.Role == UserRole.Teacher)
        {
            var teacher = await context.Teachers.AsNoTracking().FirstOrDefaultAsync(t => t.UserId == userId);

            var anClass = await context.Classes.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == existingAnnouncement.ClassId);

            // ReSharper disable once NullableWarningSuppressionIsUsed
            if (teacher!.Id != anClass!.TeacherId)
            {
                Log.Error("Teacher is not class's teacher");
                return new AnnouncementResponseDto() { Success = false, Message = "Teacher is not class's teacher" };
            }
        }

        context.Announcements.Remove(existingAnnouncement);
        await context.SaveChangesAsync();

        return new AnnouncementResponseDto() { Success = true, Message = "Announcement deleted successfully!" };
    }
}