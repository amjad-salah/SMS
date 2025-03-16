using SMS.Models.DTOs.Exams;

namespace SMS.Backend.Services.Exams;

public interface IExamsService
{
    Task<ExamsResponseDto> GetExamsAsync();
    Task<ExamsResponseDto> GetExamByIdAsync(int id);
    Task<ExamsResponseDto> ApproveExamResultAsync(int id);
    Task<ExamsResponseDto> GetExamsByCurrentYearAsync();
    Task<ExamsResponseDto> AddExamAsync(UpsertExamDto exam);
    Task<ExamsResponseDto> UpdateExamAsync(int id, UpsertExamDto exam);
    Task<ExamsResponseDto> DeleteExamAsync(int id);
}