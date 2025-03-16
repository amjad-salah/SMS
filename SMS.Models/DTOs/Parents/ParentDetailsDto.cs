using SMS.Models.DTOs.Students;

namespace SMS.Models.DTOs.Parents;

public class ParentDetailsDto : ParentDto
{
    public virtual List<StudentDto>? Students { get; set; }
}