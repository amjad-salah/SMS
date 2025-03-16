namespace SMS.Models.DTOs.Applications;

public class ApplicationsResponseDto : BaseResponseDto
{
    public List<ApplicationDto>? Applications { get; set; }
    public ApplicationDetailsDto? Application { get; set; }
}