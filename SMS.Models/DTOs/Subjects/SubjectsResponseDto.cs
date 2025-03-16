namespace SMS.Models.DTOs.Subjects;

public class SubjectsResponseDto : BaseResponseDto
{
    public List<SubjectDto>? Subjects { get; set; }
    public SubjectDetailsDto? Subject { get; set; }
}