namespace SMS.Models.DTOs.Teachers;

public class UpsertTeacherDto
{
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string? Address { get; set; } = string.Empty;
    public int InstitutionId { get; set; }
    public DateTime? JoinDate { get; set; }
    public decimal ExperienceYears { get; set; }
}