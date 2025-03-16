using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SMS.Backend.Data;
using SMS.Backend.Utils;
using SMS.Models.DTOs.Users;
using SMS.Models.Entities;

namespace SMS.Backend.Services.Users;

public class UsersService(
    AppDbContext context,
    AuthUtils authUtils,
    IValidator<AddUserDto> validator,
    IValidator<UpdateUserDto> updateUserValidator,
    IValidator<LoginRequestDto> loginValidator) : IUsersService
{
    public async Task<UsersResponseDto> GetUsersAsync()
    {
        var users = await context.Users.AsNoTracking()
            .OrderByDescending(u => u.CreatedAt)
            .ProjectToType<UserDto>().ToListAsync();

        return new UsersResponseDto() { Success = true, Users = users };
    }

    public async Task<UsersResponseDto> GetUserByIdAsync(int id)
    {
        var user = await context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);

        return user == null
            ? new UsersResponseDto() { Success = false, Message = "User not found" }
            : new UsersResponseDto() { Success = true, User = user.Adapt<UserDto>() };
    }

    public async Task<UsersResponseDto> AddUserAsync(AddUserDto user)
    {
        var validationResult = await validator.ValidateAsync(user);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));

            Log.Error(error);

            return new UsersResponseDto() { Success = false, Message = error };
        }

        var exists = await context.Users.AnyAsync(u => u.Email == user.Email);

        if (exists)
        {
            Log.Error("User with email {Email} already exists", user.Email);

            return new UsersResponseDto() { Success = false, Message = "User already exists" };
        }

        var newUser = user.Adapt<User>();

        newUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        await context.Users.AddAsync(newUser);
        await context.SaveChangesAsync();

        return new UsersResponseDto() { Success = true, Message = "User added successfully" };
    }

    public async Task<UsersResponseDto> UpdateUserAsync(int id, UpdateUserDto user)
    {
        var validationResult = await updateUserValidator.ValidateAsync(user);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));

            Log.Error(error);

            return new UsersResponseDto() { Success = false, Message = error };
        }

        var existUser = await context.Users.FindAsync(id);

        if (existUser == null)
        {
            Log.Error("User with id {Id} not found", id);

            return new UsersResponseDto() { Success = false, Message = "User not found" };
        }

        if (user.Email != existUser.Email)
        {
            var exist = await context.Users.AnyAsync(u => u.Email == user.Email);

            if (exist)
            {
                Log.Error("User with email {Email} already exists", user.Email);

                return new UsersResponseDto() { Success = false, Message = "User already exists" };
            }
        }

        existUser.Email = user.Email;
        existUser.FullName = user.FullName;
        existUser.Role = user.Role;
        existUser.InstitutionId = user.InstitutionId;
        await context.SaveChangesAsync();

        return new UsersResponseDto() { Success = true, Message = "User updated successfully" };
    }

    public async Task<UsersResponseDto> ResetPasswordAsync(int id, ResetPasswordDto newPassword)
    {
        var existUser = await context.Users.FindAsync(id);

        if (existUser == null)
        {
            Log.Error("User with id {Id} not found", id);

            return new UsersResponseDto() { Success = false, Message = "User not found" };
        }

        existUser.Password = BCrypt.Net.BCrypt.HashPassword(newPassword.NewPassword);

        await context.SaveChangesAsync();

        return new UsersResponseDto() { Success = true, Message = "Password reset successfully" };
    }

    public async Task<UsersResponseDto> DeleteUserAsync(int id)
    {
        var existUser = await context.Users.FindAsync(id);

        if (existUser == null)
        {
            Log.Error("User with id {Id} not found", id);

            return new UsersResponseDto() { Success = false, Message = "User not found" };
        }

        context.Users.Remove(existUser);
        await context.SaveChangesAsync();

        return new UsersResponseDto() { Success = true, Message = "User deleted successfully" };
    }

    public async Task<LoginResponseDto> LoginUserAsync(LoginRequestDto request)
    {
        var validationResult = await loginValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));

            Log.Error(error);

            return new LoginResponseDto() { Success = false, Message = error };
        }

        var existUser = await context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (existUser == null)
        {
            Log.Error("User with email {Email}, invalid login attempt", request.Email);

            return new LoginResponseDto() { Success = false, Message = "Invalid credentials" };
        }

        var isMatch = BCrypt.Net.BCrypt.Verify(request.Password, existUser.Password);

        if (!isMatch)
        {
            Log.Error("User with email {Email}, invalid login attempt", request.Email);

            return new LoginResponseDto() { Success = false, Message = "Invalid credentials" };
        }

        var token = authUtils.GenerateToken(existUser);

        return new LoginResponseDto()
        {
            Success = true, Token = token,
            FullName = existUser.FullName, Role = existUser.Role
        };
    }
}