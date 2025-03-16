using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Backend.Services.Users;
using SMS.Models.DTOs.Users;

namespace SMS.Backend.Controllers;

[ApiController]
[Route("api/users")]
[Authorize(Roles = "Admin")]
public class UsersController(IUsersService usersService) : ControllerBase
{
    //Get all users
    //GET /api/users
    [HttpGet("")]
    public async Task<ActionResult<UsersResponseDto>> GetUsers()
    {
        var response = await usersService.GetUsersAsync();

        return Ok(response);
    }

    //Get user by id
    //GET /api/users/:id
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UsersResponseDto>> GetUser(int id)
    {
        var response = await usersService.GetUserByIdAsync(id);

        if (!response.Success)
            return NotFound(response);

        return Ok(response);
    }

    //Add new user
    //POST /api/users
    [HttpPost("")]
    [Authorize(Roles = "Admin,Registrar")]
    public async Task<ActionResult<UsersResponseDto>> AddUser(AddUserDto userDto)
    {
        var response = await usersService.AddUserAsync(userDto);

        if (response.Success) return CreatedAtRoute("", response);

        if (response.Message == "User already exists")
            return Conflict(response);

        return BadRequest(response);
    }

    //Update user by id
    //PUT /api/users/:id
    [HttpPut("{id:int}")]
    public async Task<ActionResult<UsersResponseDto>> UpdateUser(int id, UpdateUserDto userDto)
    {
        var response = await usersService.UpdateUserAsync(id, userDto);

        if (response.Success) return Ok(response);

        if (response.Message == "User not found")
            return NotFound(response);

        if (response.Message == "User already exists")
            return Conflict(response);

        return BadRequest(response);
    }

    //Reset user's password by id
    //PUT /api/users/:id/reset
    [HttpPut("{id:int}/reset")]
    public async Task<ActionResult<UsersResponseDto>> ResetPassword(int id, ResetPasswordDto newPassword)
    {
        var response = await usersService.ResetPasswordAsync(id, newPassword);

        if (response.Success) return Ok(response);

        return NotFound(response);
    }

    //Delete user by id
    //DELETE /api/users/:id
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<UsersResponseDto>> DeleteUser(int id)
    {
        var response = await usersService.DeleteUserAsync(id);

        if (!response.Success) return NotFound(response);

        return Ok(response);
    }

    //Login user
    //POST /api/users/login
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponseDto>> LoginUser(LoginRequestDto request)
    {
        var response = await usersService.LoginUserAsync(request);

        if (response.Success) return Ok(response);

        if (response.Message == "Invalid credentials")
            return Unauthorized(response);

        return BadRequest(response);
    }
}