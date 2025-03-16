using SMS.Models.DTOs.Subjects;

namespace SMS.Backend.Services.Subjects;

public interface ISubjectsService
{
    Task<SubjectsResponseDto> GetSubjectsAsync();
    Task<SubjectsResponseDto> GetSubjectByIdAsync(int id);
    Task<SubjectsResponseDto> AddSubjectAsync(UpsertSubjectDto subject);
    Task<SubjectsResponseDto> UpdateSubjectAsync(int id, UpsertSubjectDto subject);
    Task<SubjectsResponseDto> DeleteSubjectAsync(int id);
}