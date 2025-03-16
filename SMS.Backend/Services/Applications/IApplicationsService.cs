using SMS.Models.DTOs.Applications;

namespace SMS.Backend.Services.Applications;

public interface IApplicationsService
{
    Task<ApplicationsResponseDto> GetApplicationsAsync();
    Task<ApplicationsResponseDto> GetApplicationByIdAsync(int id);
    Task<ApplicationsResponseDto> GetApplicationsByCurrentYearAsync();
    Task<ApplicationsResponseDto> AddApplicationAsync(UpsertApplicationDto application);
    Task<ApplicationsResponseDto> UpdateApplicationAsync(int id, UpsertApplicationDto application);
    Task<ApplicationsResponseDto> DeleteApplicationAsync(int id);
    Task<ApplicationApproverResponseDto> ApproveApplicationsAsync(int id, ApplicationApproveDto approveDto);
}