using SMS.Models.DTOs.Grades;
using SMS.Models.DTOs.Institutions;
using SMS.Models.DTOs.Students;
using SMS.Models.DTOs.Teachers;

namespace SMS.Models.DTOs.Classes;

public class ClassDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int GradeId { get; set; }
    public virtual GradeDto? Grade { get; set; }
    public int InstitutionId { get; set; }
    public virtual InstitutionDto? Institution { get; set; }
    public int TeacherId { get; set; }
    public int Capacity { get; set; }
    public virtual TeacherDto? Teacher { get; set; }
    public virtual List<StudentDto>? Students { get; set; }
}