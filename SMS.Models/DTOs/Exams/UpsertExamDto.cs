namespace SMS.Models.DTOs.Exams;

public class UpsertExamDto
{
    public string Title { get; set; } = string.Empty;
    public DateTime ExamDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int ExamTypeId { get; set; }
    public decimal MaxMark { get; set; }
    public decimal MinMark { get; set; }
    public int GradeId { get; set; }
    public int AcademicYearId { get; set; }
}