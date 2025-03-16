namespace SMS.Models.DTOs.ExamTypes;

public class ExamTypesResponseDto : BaseResponseDto
{
    public List<ExamTypeDto>? ExamTypes { get; set; }
    public ExamTypeDto? ExamType { get; set; }
}