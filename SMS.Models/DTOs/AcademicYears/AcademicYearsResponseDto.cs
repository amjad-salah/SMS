namespace SMS.Models.DTOs.AcademicYears;

public class AcademicYearsResponseDto : BaseResponseDto
{
    public List<AcademicYearDto>? AcademicYears { get; set; }
    public AcademicYearDetailsDto? AcademicYear { get; set; }
}