using SMS.Models.DTOs.Fees;

namespace SMS.Backend.Services.Fees;

public interface IFeesService
{
    Task<FeesResponseDto> GetFeesAsync();
    Task<FeesResponseDto> GetFeesByIdAsync(int id);
    Task<FeesResponseDto> AddFeeAsync(UpsertFeeDto fee);
    Task<FeesResponseDto> DeleteFeeAsync(int id);
}