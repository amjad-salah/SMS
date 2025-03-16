using SMS.Models.DTOs.Users;

namespace SMS.Backend.Services.Users;

public interface IUsersService
{
    Task<UsersResponseDto> GetUsersAsync();
    Task<UsersResponseDto> GetUserByIdAsync(int id);
    Task<UsersResponseDto> AddUserAsync(AddUserDto user);
    Task<UsersResponseDto> UpdateUserAsync(int id, UpdateUserDto user);
    Task<UsersResponseDto> ResetPasswordAsync(int id, ResetPasswordDto newPassword);
    Task<UsersResponseDto> DeleteUserAsync(int id);
    Task<LoginResponseDto> LoginUserAsync(LoginRequestDto request);
}