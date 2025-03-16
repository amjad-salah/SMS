using SMS.Models.DTOs.Classes;
using SMS.Models.DTOs.Lessons;

namespace SMS.Models.DTOs.Teachers;

public class TeacherDetailsDto : TeacherDto
{
    public DateOnly JoinDate { get; set; }
    public decimal ExperienceYears { get; set; }
    public virtual List<ClassDto>? Classes { get; set; }
    public virtual List<LessonDto>? Lessons { get; set; }
}