namespace SMS.Models.DTOs.Students;

public class TransferStudentsDto
{
    public List<int> StudentIds { get; set; } = [];
    public int ClassId { get; set; }
}