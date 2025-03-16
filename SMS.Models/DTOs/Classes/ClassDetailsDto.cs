using SMS.Models.DTOs.Announcements;
using SMS.Models.DTOs.Assignments;
using SMS.Models.DTOs.Attendances;
using SMS.Models.DTOs.Lessons;

namespace SMS.Models.DTOs.Classes;

public class ClassDetailsDto : ClassDto
{
    public virtual List<LessonDto>? Lessons { get; set; }
    public virtual List<AssignmentDto>? Assignments { get; set; }
    public virtual List<AnnouncementDto>? Announcements { get; set; }
    public virtual List<AttendanceDto>? Attendances { get; set; }
}