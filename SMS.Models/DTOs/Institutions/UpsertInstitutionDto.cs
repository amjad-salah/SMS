using SMS.Models.Entities;

namespace SMS.Models.DTOs.Institutions;

public class UpsertInstitutionDto
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public InstitutionType InstitutionType { get; set; }
}