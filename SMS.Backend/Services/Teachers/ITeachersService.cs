using SMS.Models.DTOs.Teachers;

namespace SMS.Backend.Services.Teachers;

public interface ITeachersService
{
    Task<TeachersResponseDto> GetTeachersAsync();
    Task<TeachersResponseDto> GetTeacherByIdAsync(int id, int userId);
    Task<TeachersResponseDto> AddTeacherAsync(UpsertTeacherDto teacher);
    Task<TeachersResponseDto> UpdateTeacherAsync(int id, UpsertTeacherDto teacher);
    Task<TeachersResponseDto> DeleteTeacherAsync(int id);
}