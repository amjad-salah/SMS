using SMS.Models.DTOs.Classes;
using SMS.Models.DTOs.Subjects;
using SMS.Models.DTOs.Teachers;

namespace SMS.Models.DTOs.Lessons;

public class LessonDto
{
    public int Id { get; set; }
    public DayOfWeek Day { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int SubjectId { get; set; }
    public int ClassId { get; set; }
    public virtual ClassDto? Class { get; set; }
    public int TeacherId { get; set; }
    public virtual SubjectDto? Subject { get; set; }
    public virtual TeacherDto? Teacher { get; set; }
}