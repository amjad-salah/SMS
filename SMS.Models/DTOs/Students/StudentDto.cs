using SMS.Models.DTOs.AcademicYears;
using SMS.Models.DTOs.Classes;
using SMS.Models.DTOs.Grades;
using SMS.Models.Entities;

namespace SMS.Models.DTOs.Students;

public class StudentDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public int GradeId { get; set; }
    public virtual GradeDto? Grade { get; set; }
    public string StudentNo { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public StudentStatus Status { get; set; }
    public int AcademicYearId { get; set; }
    public virtual AcademicYearDto? AcademicYear { get; set; }
}