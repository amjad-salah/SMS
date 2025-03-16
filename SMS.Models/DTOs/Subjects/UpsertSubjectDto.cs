namespace SMS.Models.DTOs.Subjects;

public class UpsertSubjectDto
{
    public string Name { get; set; } = string.Empty;
    public int GradeId { get; set; }
}