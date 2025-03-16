namespace SMS.Models.DTOs.Users;

public class UsersResponseDto : BaseResponseDto
{
    public List<UserDto>? Users { get; set; }
    public UserDto? User { get; set; }
}