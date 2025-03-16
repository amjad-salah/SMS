using SMS.Models.DTOs.Classes;
using SMS.Models.DTOs.Students;
using SMS.Models.DTOs.Teachers;
using SMS.Models.DTOs.Users;

namespace SMS.Models.DTOs.Institutions;

public class InstitutionDetailsDto : InstitutionDto
{
    public List<UserDto>? Users { get; set; }
    public List<TeacherDto>? Teachers { get; set; }
    public List<StudentDto>? Students { get; set; }
    public List<ClassDto>? Classes { get; set; }
}