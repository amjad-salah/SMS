namespace SMS.Models.DTOs.ExamResults;

public class ExamResultsResponseDto : BaseResponseDto
{
    public List<ExamResultDto>? Results { get; set; }
    public ExamResultDto? Result { get; set; }
}