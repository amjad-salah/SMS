namespace SMS.Models.DTOs.Institutions;

public class InstitutionsResponseDto : BaseResponseDto
{
    public List<InstitutionDto>? Institutions { get; set; }
    public InstitutionDetailsDto? Institution { get; set; }
}