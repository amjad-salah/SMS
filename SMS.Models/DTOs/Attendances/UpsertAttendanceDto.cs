namespace SMS.Models.DTOs.Attendances;

public class UpsertAttendanceDto
{
    public DateTime Date { get; set; }
    public bool Present { get; set; }
    public int ClassId { get; set; }
    public int StudentId { get; set; }
}