namespace SMS.Models.DTOs.Classes;

public class ClassesResponseDto : BaseResponseDto
{
    public List<ClassDto>? Classes { get; set; }
    public ClassDetailsDto? Class { get; set; }
}