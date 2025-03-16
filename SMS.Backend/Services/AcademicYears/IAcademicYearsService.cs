using SMS.Models.DTOs.AcademicYears;

namespace SMS.Backend.Services.AcademicYears;

public interface IAcademicYearsService
{
    Task<AcademicYearsResponseDto> GetAcademicYearsAsync();
    Task<AcademicYearsResponseDto> GetAcademicYearByIdAsync(int id);
    Task<AcademicYearsResponseDto> AddAcademicYearAsync(UpsertAcademicYearDto year);
    Task<AcademicYearsResponseDto> UpdateAcademicYearAsync(int id, UpsertAcademicYearDto year);
    Task<AcademicYearsResponseDto> DeleteAcademicYearAsync(int id);
}