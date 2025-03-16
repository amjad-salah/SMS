using SMS.Models.DTOs.Classes;
using SMS.Models.DTOs.Students;

namespace SMS.Models.DTOs.Dashboards.Parent;

public class ParentDashboardDto : BaseResponseDto
{
    public List<StudentDto> Students { get; set; } = [];
    public List<ClassDetailsDto> Classes { get; set; } = [];
}