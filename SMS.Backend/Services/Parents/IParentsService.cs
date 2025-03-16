
using SMS.Models.DTOs.Parents;

namespace SMS.Backend.Services.Parents;

public interface IParentsService
{
    Task<ParentsResponseDto> GetParentsAsync();
    Task<ParentsResponseDto> GetParentByIdAsync(int id);
    Task<ParentsResponseDto> AddParentAsync(UpsertParentDto parent);
    Task<ParentsResponseDto> UpdateParentAsync(int id, UpsertParentDto parent);
    Task<ParentsResponseDto> DeleteParentAsync(int id);
}