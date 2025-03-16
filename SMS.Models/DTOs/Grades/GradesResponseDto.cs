namespace SMS.Models.DTOs.Grades;

public class GradesResponseDto : BaseResponseDto
{
    public List<GradeDto>? Grades { get; set; }
    public GradeDetailsDto? Grade { get; set; }
}