using SMS.Models.DTOs.Lessons;

namespace SMS.Backend.Services.Lessons;

public interface ILessonsService
{
    Task<LessonsResponseDto> GetLessonsAsync();
    Task<LessonsResponseDto> GetLessonByIdAsync(int id);
    Task<LessonsResponseDto> AddLessonAsync(UpsertLessonDto lesson);
    Task<LessonsResponseDto> UpdateLessonAsync(int id, UpsertLessonDto lesson);
    Task<LessonsResponseDto> DeleteLessonAsync(int id);
}