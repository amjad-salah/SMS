using SMS.Models.DTOs.Students;

namespace SMS.Backend.Services.Students;

public interface IStudentsService
{
    Task<StudentsResponseDto> GetStudentsAsync();
    Task<StudentsResponseDto> GetStudentByIdAsync(int id, int userId);
    Task<StudentsResponseDto> GetActiveStudentsAsync();
    Task<StudentsResponseDto> AddStudentAsync(UpsertStudentDto student);
    Task<StudentsResponseDto> UpdateStudentAsync(int id, UpsertStudentDto student);
    Task<StudentsResponseDto> DeleteStudentAsync(int id);
    Task<StudentsResponseDto> TransferStudentAsync(int studentId, int classId, int userId);
    Task<StudentsResponseDto> TransferStudentsAsync(List<int> studentIds, int classId, int userId);
}