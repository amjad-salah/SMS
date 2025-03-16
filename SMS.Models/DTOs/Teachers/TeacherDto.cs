using SMS.Models.DTOs.Institutions;

namespace SMS.Models.DTOs.Teachers;

public class TeacherDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string? Address { get; set; } = string.Empty;
    public int InstitutionId { get; set; }
    public virtual InstitutionDto? Institution { get; set; }
}