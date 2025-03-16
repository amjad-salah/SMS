using SMS.Models.DTOs.Attendances;

namespace SMS.Backend.Services.Attendances;

public interface IAttendancesService
{
    Task<AttendanceResponseDto> GetAttendancesAsync();
    Task<AttendanceResponseDto> AddAttendanceAsync(UpsertAttendanceDto attendance);
    Task<AttendanceResponseDto> UpdateAttendancePresentAsync(int id);
    Task<AttendanceResponseDto> AddClassAttendances(AddClassAttendancesDto request);
}