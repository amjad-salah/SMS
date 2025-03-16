using SMS.Models.DTOs.ExamResults;

namespace SMS.Backend.Services.ExamResults;

public interface IExamResultsService
{
    Task<ExamResultsResponseDto> GetResultsAsync();
    Task<ExamResultsResponseDto> GetResultByIdAsync(int id);
    Task<ExamResultsResponseDto> GetResultByCurrentYearAsync();
    Task<ExamResultsResponseDto> AddResultAsync(UpsertExamResultDto result);
    Task<ExamResultsResponseDto> UpdateResultAsync(int id, UpsertExamResultDto result);
    Task<ExamResultsResponseDto> DeleteResultAsync(int id);
}