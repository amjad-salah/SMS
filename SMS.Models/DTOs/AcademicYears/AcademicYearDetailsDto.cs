using SMS.Models.DTOs.Exams;
using SMS.Models.DTOs.Students;

namespace SMS.Models.DTOs.AcademicYears;

public class AcademicYearDetailsDto : AcademicYearDto
{
    public List<ExamDto>? Exams { get; set; }
    public List<StudentDto>? Students { get; set; }
}