using SMS.Models.DTOs.Classes;

namespace SMS.Backend.Services.Classes;

public interface IClassesService
{
    Task<ClassesResponseDto> GetClassesAsync();
    Task<ClassesResponseDto> GetClassByIdAsync(int id);
    Task<ClassesResponseDto> AddClassAsync(UpsertClassDto classDto);
    Task<ClassesResponseDto> UpdateClassAsync(int id, UpsertClassDto classDto);
    Task<ClassesResponseDto> DeleteClassAsync(int id);
}