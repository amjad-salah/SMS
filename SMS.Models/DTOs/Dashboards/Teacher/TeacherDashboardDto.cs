using SMS.Models.DTOs.Classes;
using SMS.Models.DTOs.Teachers;

namespace SMS.Models.DTOs.Dashboards.Teacher;

public class TeacherDashboardDto : BaseResponseDto
{
    public TeacherDetailsDto Teacher { get; set; } = new();
    public List<ClassDetailsDto> Classes { get; set; } = [];
}