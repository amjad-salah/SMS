using SMS.Models.DTOs.Attendances;
using SMS.Models.DTOs.ExamResults;
using SMS.Models.DTOs.Institutions;
using SMS.Models.DTOs.Invoices;
using SMS.Models.DTOs.Parents;
using SMS.Models.DTOs.StudentRecords;
using SMS.Models.DTOs.Users;

namespace SMS.Models.DTOs.Students;

public class StudentDetailsDto : StudentDto
{
    public virtual List<StudentRecordDto>? Records { get; set; }
    public virtual List<ExamResultDto>? ExamResults { get; set; }
    public virtual List<AttendanceDto>? Attendances { get; set; }
    public virtual List<InvoiceDto>? Invoices { get; set; }
    public DateOnly BirthDate { get; set; }
    public int ParentId { get; set; }
    public virtual ParentDto? Parent { get; set; }
    public DateOnly AdmissionDate { get; set; }
    public string? MedicalInfo { get; set; }
    public int? UserId { get; set; }
    public virtual UserDto? User { get; set; }
    public int InstitutionId { get; set; }
    public virtual InstitutionDto? Institution { get; set; }
}