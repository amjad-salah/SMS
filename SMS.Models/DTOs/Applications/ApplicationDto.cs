using SMS.Models.DTOs.Institutions;
using SMS.Models.Entities;

namespace SMS.Models.DTOs.Applications;

public class ApplicationDto
{
    public int Id { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string GuardianName { get; set; } = string.Empty;
    public string ApplicationNo { get; set; } = string.Empty;
    public string InstitutionId { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public InstitutionDto? Institution { get; set; }
    public ApplicationStatus Status { get; set; }
}