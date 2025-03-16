namespace SMS.Models.DTOs.Assignments;

public class AssignmentResponseDto : BaseResponseDto
{
    public List<AssignmentDto>? Assignments { get; set; }
    public AssignmentDto? Assignment { get; set; }
}