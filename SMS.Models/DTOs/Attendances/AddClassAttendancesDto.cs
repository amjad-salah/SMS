namespace SMS.Models.DTOs.Attendances;

public class AddClassAttendancesDto
{
    public int ClassId { get; set; }
    public DateOnly? Date { get; set; }
}