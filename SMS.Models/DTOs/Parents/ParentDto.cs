using SMS.Models.DTOs.Users;

namespace SMS.Models.DTOs.Parents;

public class ParentDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int? UserId { get; set; }
    public virtual UserDto? User { get; set; }
}