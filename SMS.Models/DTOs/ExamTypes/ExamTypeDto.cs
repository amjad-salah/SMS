using SMS.Models.DTOs.Exams;

namespace SMS.Models.DTOs.ExamTypes;

public class ExamTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<ExamDto>? Exams { get; set; }
}