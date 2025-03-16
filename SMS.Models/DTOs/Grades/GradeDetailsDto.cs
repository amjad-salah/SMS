using SMS.Models.DTOs.Classes;
using SMS.Models.DTOs.Exams;
using SMS.Models.DTOs.Fees;
using SMS.Models.DTOs.Students;
using SMS.Models.DTOs.Subjects;

namespace SMS.Models.DTOs.Grades;

public class GradeDetailsDto : GradeDto
{
    public FeeDto? Fee { get; set; }
    public List<StudentDto>? Students { get; set; }
    public List<ClassDto>? Classes { get; set; }
    public List<ExamDto>? Exams { get; set; }
    public List<SubjectDto>? Subjects { get; set; }
}