namespace SMS.Models.DTOs.Teachers;

public class TeachersResponseDto : BaseResponseDto
{
    public List<TeacherDto>? Teachers { get; set; }
    public TeacherDetailsDto? Teacher { get; set; }
}