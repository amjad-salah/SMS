namespace SMS.Models.DTOs.Attendances;

public class AttendanceResponseDto : BaseResponseDto
{
    public List<AttendanceDto>? Attendances { get; set; }
}