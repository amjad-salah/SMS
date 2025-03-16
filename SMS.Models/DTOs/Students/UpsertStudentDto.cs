using SMS.Models.Entities;

namespace SMS.Models.DTOs.Students;

public class UpsertStudentDto
{
    public string FullName { get; set; } = string.Empty;
    public int GradeId { get; set; }
    public int ClassId { get; set; }
    public string StudentNo { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public StudentStatus Status { get; set; }
    public int AcademicYearId { get; set; }
    public DateOnly BirthDate { get; set; }
    public int ParentId { get; set; }
    public DateOnly AdmissionDate { get; set; }
    public string? MedicalInfo { get; set; }
    public int InstitutionId { get; set; }
}