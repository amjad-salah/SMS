using SMS.Models.DTOs.AcademicYears;
using SMS.Models.DTOs.Grades;
using SMS.Models.DTOs.Students;
using SMS.Models.Entities;

namespace SMS.Models.DTOs.StudentRecords;

public class StudentRecordDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public virtual StudentDto? Student { get; set; }
    public int GradeId { get; set; }
    public virtual GradeDto? Grade { get; set; }
    public int AcademicYearId { get; set; }
    public virtual AcademicYearDto? AcademicYear { get; set; }
    public Result? Result { get; set; }
    public string? Remarks { get; set; }
    public string? TotalRemark { get; set; }
    public string? Percentage { get; set; }
    public string? Notes { get; set; }
}