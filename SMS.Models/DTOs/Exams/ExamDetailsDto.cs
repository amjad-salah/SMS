using SMS.Models.DTOs.ExamResults;

namespace SMS.Models.DTOs.Exams;

public class ExamDetailsDto : ExamDto
{
    public List<ExamResultDto>? Results { get; set; }
}