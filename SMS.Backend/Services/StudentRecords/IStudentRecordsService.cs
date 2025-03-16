using SMS.Models.DTOs.StudentRecords;

namespace SMS.Backend.Services.StudentRecords;

public interface IStudentRecordsService
{
    Task<StudentRecordsResponseDto> GetRecordsAsync();
    Task<StudentRecordsResponseDto> GetRecordsByStudentAsync(int studentId, int userId);
    Task<StudentRecordsResponseDto> GetRecordsByCurrentYearAsync();
    Task<StudentRecordsResponseDto> AddRecordsAsync(UpsertStudentRecordDto record);
    Task<StudentRecordsResponseDto> DeleteRecordsAsync(int id);
    Task<StudentRecordsResponseDto> AddStudentsRecordsAsync(List<int> studentIds, int examTypeId);
}