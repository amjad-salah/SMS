using SMS.Models.Entities;

namespace SMS.Models.DTOs.Applications;

public class UpsertApplicationDto
{
    public string StudentName { get; set; } = string.Empty;
    public string GuardianName { get; set; } = string.Empty;
    public string GuardianPhone { get; set; } = string.Empty;
    public string GuardianAddress { get; set; } = string.Empty;
    public string? GuardianEmail { get; set; } = string.Empty;
    public int GradeId { get; set; }
    public Gender Gender { get; set; }
    public DateTime BirthDate { get; set; }
    public int AcademicYearId { get; set; }
    public int InstitutionId { get; set; }
    public ApplicationStatus Status { get; set; }
}