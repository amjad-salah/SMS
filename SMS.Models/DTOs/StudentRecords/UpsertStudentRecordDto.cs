using SMS.Models.Entities;

namespace SMS.Models.DTOs.StudentRecords;

public class UpsertStudentRecordDto
{
    public int StudentId { get; set; }
    public int GradeId { get; set; }
    public int AcademicYearId { get; set; }
    public Result? Result { get; set; }
    public string? Remarks { get; set; }
    public string? TotalRemark { get; set; }
    public string? Percentage { get; set; }
    public string? Notes { get; set; }
}