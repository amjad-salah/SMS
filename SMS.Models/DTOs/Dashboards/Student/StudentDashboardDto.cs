using SMS.Models.DTOs.Classes;
using SMS.Models.DTOs.Students;

namespace SMS.Models.DTOs.Dashboards.Student;

public class StudentDashboardDto : BaseResponseDto
{
    public StudentDetailsDto Student { get; set; } = new();
    public ClassDetailsDto Class { get; set; } = new();
}