namespace SMS.Models.DTOs.Lessons;

public class LessonsResponseDto : BaseResponseDto
{
    public List<LessonDto>? Lessons { get; set; }
    public LessonDto? Lesson { get; set; }
}