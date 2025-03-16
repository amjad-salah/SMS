using SMS.Models.DTOs.Grades;

namespace SMS.Models.DTOs.Subjects;

public class SubjectDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int GradeId { get; set; }
    public GradeDto? Grade { get; set; }
}