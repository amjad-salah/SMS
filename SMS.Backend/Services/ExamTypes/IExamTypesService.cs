using SMS.Models.DTOs.ExamTypes;

namespace SMS.Backend.Services.ExamTypes;

public interface IExamTypesService
{
    Task<ExamTypesResponseDto> GetExamTypesAsync();
    Task<ExamTypesResponseDto> GetExamTypeByIdAsync(int id);
    Task<ExamTypesResponseDto> AddExamTypeAsync(UpsertExamTypeDto examType);
    Task<ExamTypesResponseDto> DeleteExamTypeAsync(int id);
}