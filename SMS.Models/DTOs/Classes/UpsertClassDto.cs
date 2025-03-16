namespace SMS.Models.DTOs.Classes;

public class UpsertClassDto
{
    public string Name { get; set; } = string.Empty;
    public int GradeId { get; set; }
    public int InstitutionId { get; set; }
    public int TeacherId { get; set; }
    public int Capacity { get; set; }
}