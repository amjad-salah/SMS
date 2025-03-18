namespace SMS.Models.DTOs.Assignments;

public class UpsertAssignmentDto
{
    public string Title { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int ClassId { get; set; }
}