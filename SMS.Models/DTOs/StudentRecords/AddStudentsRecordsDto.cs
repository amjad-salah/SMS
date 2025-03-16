namespace SMS.Models.DTOs.StudentRecords;

public class AddStudentsRecordsDto
{
    public List<int> StudentIds { get; set; } = [];
    public int ExamTypeId { get; set; }
}