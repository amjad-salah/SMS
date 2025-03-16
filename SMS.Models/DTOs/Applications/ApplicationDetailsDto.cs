using SMS.Models.DTOs.AcademicYears;
using SMS.Models.DTOs.Grades;

namespace SMS.Models.DTOs.Applications;

public class ApplicationDetailsDto : ApplicationDto
{
    public string GuardianPhone { get; set; } = string.Empty;
    public string? GuardianEmail { get; set; } = string.Empty;
    public string? GuardianAddress { get; set; } = string.Empty;
    public int GradeId { get; set; }
    public GradeDto? Grade { get; set; }
    public DateOnly BirthDate { get; set; }
    public int AcademicYearId { get; set; }
    public AcademicYearDto? AcademicYear { get; set; }
}