namespace SMS.Models.DTOs.Assignments;

public class UpsertAssignmentDto
{
    public string Title { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int ClassId { get; set; }
}