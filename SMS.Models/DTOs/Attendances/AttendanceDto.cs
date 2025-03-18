using SMS.Models.DTOs.Classes;
using SMS.Models.DTOs.Students;

namespace SMS.Models.DTOs.Attendances;

public class AttendanceDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public bool Present { get; set; }
    public int ClassId { get; set; }
    public int StudentId { get; set; }
    public virtual ClassDto? Class { get; set; }
    public virtual StudentDto? Student { get; set; }
}