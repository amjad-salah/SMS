using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SMS.Backend.Data;
using SMS.Models.DTOs.Teachers;
using SMS.Models.Entities;

namespace SMS.Backend.Services.Teachers;

public class TeachersService(
    AppDbContext context,
    IValidator<UpsertTeacherDto> validator) : ITeachersService
{
    public async Task<TeachersResponseDto> GetTeachersAsync()
    {
        var teachers = await context.Teachers.AsNoTracking()
            .OrderByDescending(t => t.CreatedAt)
            .Include(t => t.Institution)
            .ProjectToType<TeacherDto>()
            .ToListAsync();

        return new TeachersResponseDto() { Success = true, Teachers = teachers };
    }

    public async Task<TeachersResponseDto> GetTeacherByIdAsync(int id, int userId)
    {
        var teacher = await context.Teachers.AsNoTracking()
            .Include(t => t.Institution)
            .Include(t => t.Classes)
            .Include(t => t.Lessons)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (teacher == null)
            return new TeachersResponseDto() { Success = true, Message = "Teacher not found" };

        var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);

        if (user is { Role: UserRole.Teacher } && user.Id != teacher.UserId)
            return new TeachersResponseDto()
            {
                Success = false,
                Message = "You don't have permission to view this teacher"
            };

        return new TeachersResponseDto() { Success = true, Teacher = teacher.Adapt<TeacherDetailsDto>() };
    }

    public async Task<TeachersResponseDto> AddTeacherAsync(UpsertTeacherDto teacher)
    {
        var validationResult = await validator.ValidateAsync(teacher);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));

            Log.Error(error);

            return new TeachersResponseDto() { Success = false, Message = error };
        }

        var newTeacher = teacher.Adapt<Teacher>();
        await context.Teachers.AddAsync(newTeacher);
        await context.SaveChangesAsync();

        return new TeachersResponseDto()
        {
            Success = true, Message = "Teacher added successfully!",
            Teacher = newTeacher.Adapt<TeacherDetailsDto>()
        };
    }

    public async Task<TeachersResponseDto> UpdateTeacherAsync(int id, UpsertTeacherDto teacher)
    {
        var existingTeacher = await context.Teachers.FindAsync(id);

        if (existingTeacher == null)
            return new TeachersResponseDto() { Success = false, Message = "Teacher not found" };

        var validationResult = await validator.ValidateAsync(teacher);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));

            Log.Error(error);

            return new TeachersResponseDto() { Success = false, Message = error };
        }

        existingTeacher.FullName = teacher.FullName;
        existingTeacher.Phone = teacher.Phone;
        existingTeacher.Email = teacher.Email;
        existingTeacher.InstitutionId = teacher.InstitutionId;
        existingTeacher.Address = teacher.Address;
        existingTeacher.JoinDate = teacher.JoinDate;
        existingTeacher.ExperienceYears = teacher.ExperienceYears;

        await context.SaveChangesAsync();

        return new TeachersResponseDto() { Success = true, Message = "Teacher updated successfully!" };
    }

    public async Task<TeachersResponseDto> DeleteTeacherAsync(int id)
    {
        var existingTeacher = await context.Teachers.FindAsync(id);

        if (existingTeacher == null)
            return new TeachersResponseDto() { Success = false, Message = "Teacher not found" };

        context.Teachers.Remove(existingTeacher);
        await context.SaveChangesAsync();

        return new TeachersResponseDto() { Success = true, Message = "Teacher deleted successfully!" };
    }
}