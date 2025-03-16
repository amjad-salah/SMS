using SMS.Models.DTOs.Institutions;

namespace SMS.Backend.Services.Institutions;

public interface IInstitutionService
{
    Task<InstitutionsResponseDto> GetInstitutionsAsync();
    Task<InstitutionsResponseDto> GetInstitutionByIdAsync(int id);
    Task<InstitutionsResponseDto> AddInstitutionAsync(UpsertInstitutionDto institution);
    Task<InstitutionsResponseDto> UpdateInstitutionAsync(int id, UpsertInstitutionDto institution);
    Task<InstitutionsResponseDto> DeleteInstitutionAsync(int id);
}