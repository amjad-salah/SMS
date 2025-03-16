namespace SMS.Models.DTOs.Exams;

public class ExamsResponseDto : BaseResponseDto
{
    public List<ExamDto>? Exams { get; set; }
    public ExamDetailsDto? Exam { get; set; }
}