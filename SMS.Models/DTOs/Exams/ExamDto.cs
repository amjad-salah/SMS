using SMS.Models.DTOs.AcademicYears;
using SMS.Models.DTOs.ExamTypes;
using SMS.Models.DTOs.Grades;

namespace SMS.Models.DTOs.Exams;

public class ExamDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime ExamDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int ExamTypeId { get; set; }
    public virtual ExamTypeDto? ExamType { get; set; }
    public decimal MaxMark { get; set; }
    public decimal MinMark { get; set; }
    public int GradeId { get; set; }
    public virtual GradeDto? Grade { get; set; }
    public int AcademicYearId { get; set; }
    public virtual AcademicYearDto? AcademicYear { get; set; }
}