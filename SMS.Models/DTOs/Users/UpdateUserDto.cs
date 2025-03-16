using SMS.Models.Entities;

namespace SMS.Models.DTOs.Users;

public class UpdateUserDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public int InstitutionId { get; set; }
}