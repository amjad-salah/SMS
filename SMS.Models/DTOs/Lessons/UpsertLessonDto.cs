namespace SMS.Models.DTOs.Lessons;

public class UpsertLessonDto
{
    public DayOfWeek Day { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public int SubjectId { get; set; }
    public int ClassId { get; set; }
    public int TeacherId { get; set; }
}