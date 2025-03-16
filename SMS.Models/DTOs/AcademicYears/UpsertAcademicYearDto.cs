namespace SMS.Models.DTOs.AcademicYears;

public class UpsertAcademicYearDto
{
    public string Name { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public bool IsCurrent { get; set; }
}