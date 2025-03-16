namespace SMS.Models.DTOs.StudentRecords;

public class StudentRecordsResponseDto : BaseResponseDto
{
    public List<StudentRecordDto>? Records { get; set; }
    public StudentRecordDto? Record { get; set; }
}