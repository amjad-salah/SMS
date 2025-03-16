using SMS.Models.DTOs.Lessons;

namespace SMS.Models.DTOs.Subjects;

public class SubjectDetailsDto : SubjectDto
{
    public List<LessonDto>? Lessons { get; set; }
}