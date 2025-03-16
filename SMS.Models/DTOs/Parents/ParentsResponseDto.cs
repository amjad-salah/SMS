namespace SMS.Models.DTOs.Parents;

public class ParentsResponseDto : BaseResponseDto
{
    public List<ParentDto>? Parents { get; set; }
    public ParentDetailsDto? Parent { get; set; }
}