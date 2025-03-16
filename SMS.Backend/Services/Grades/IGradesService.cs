using SMS.Models.DTOs.Grades;

namespace SMS.Backend.Services.Grades;

public interface IGradesService
{
    Task<GradesResponseDto> GetGradesAsync();
    Task<GradesResponseDto> GetGradeByIdAsync(int id);
    Task<GradesResponseDto> AddGradeAsync(UpsertGradeDto grade);
    Task<GradesResponseDto> UpdateGradeAsync(int id, UpsertGradeDto grade);
    Task<GradesResponseDto> DeleteGradeAsync(int id);
}