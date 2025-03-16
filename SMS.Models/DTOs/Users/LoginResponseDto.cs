using SMS.Models.Entities;

namespace SMS.Models.DTOs.Users;

public class LoginResponseDto : BaseResponseDto
{
    public string? Token { get; set; }
    public string? FullName { get; set; }
    public UserRole? Role { get; set; }
}