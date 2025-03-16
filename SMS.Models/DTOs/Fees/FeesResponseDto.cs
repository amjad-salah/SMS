namespace SMS.Models.DTOs.Fees;

public class FeesResponseDto : BaseResponseDto
{
    public List<FeeDto>? Fees { get; set; }
    public FeeDto? Fee { get; set; }
}