using SMS.Models.DTOs.Classes;

namespace SMS.Models.DTOs.Assignments;

public class AssignmentDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int ClassId { get; set; }
    public virtual ClassDto? Class { get; set; }
}