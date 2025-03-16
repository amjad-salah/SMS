using SMS.Models.DTOs.Assignments;

namespace SMS.Backend.Services.Assignments;

public interface IAssignmentsService
{
    Task<AssignmentResponseDto> GetAssignmentsAsync();
    Task<AssignmentResponseDto> GetAssignmentByIdAsync(int id);
    Task<AssignmentResponseDto> AddAssignmentAsync(UpsertAssignmentDto assignment, int userId);
    Task<AssignmentResponseDto> UpdateAssignmentAsync(int id, UpsertAssignmentDto assignment);
    Task<AssignmentResponseDto> DeleteAssignmentAsync(int id);
}