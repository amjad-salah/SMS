namespace SMS.Models.DTOs.ExamResults;

public class UpsertExamResultDto
{
    public decimal Score { get; set; }
    public decimal Percentage { get; set; }
    public int ExamId { get; set; }
    public int StudentId { get; set; }
}