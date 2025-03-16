using SMS.Models.DTOs.Exams;
using SMS.Models.DTOs.Students;

namespace SMS.Models.DTOs.ExamResults;

public class ExamResultDto
{
    public int Id { get; set; }
    public decimal Score { get; set; }
    public decimal Percentage { get; set; }
    public bool Approved { get; set; }
    public int ExamId { get; set; }
    public virtual ExamDto? Exam { get; set; }
    public int StudentId { get; set; }
    public virtual StudentDto? Student { get; set; }
}