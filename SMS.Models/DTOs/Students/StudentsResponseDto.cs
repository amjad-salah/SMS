namespace SMS.Models.DTOs.Students;

public class StudentsResponseDto : BaseResponseDto
{
    public List<StudentDto>? Students { get; set; }
    public StudentDetailsDto? Student { get; set; }
}