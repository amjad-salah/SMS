using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SMS.Backend.Data;
using SMS.Models.DTOs.Classes;
using SMS.Models.Entities;

namespace SMS.Backend.Services.Classes;

public class ClassesService(
    AppDbContext context,
    IValidator<UpsertClassDto> validator) : IClassesService
{
    public async Task<ClassesResponseDto> GetClassesAsync()
    {
        var classes = await context.Classes.AsNoTracking()
            .Include(c => c.Teacher)
            .Include(c => c.Students)
            .Include(c => c.Institution)
            .Include(c => c.Grade)
            .OrderByDescending(c => c.CreatedAt)
            .ProjectToType<ClassDto>()
            .ToListAsync();

        return new ClassesResponseDto() { Success = true, Classes = classes };
    }

    public async Task<ClassesResponseDto> GetClassByIdAsync(int id)
    {
        var existClass = await context.Classes.AsNoTracking()
            .Include(c => c.Teacher)
            .Include(c => c.Students)
            .Include(c => c.Announcements)
            .Include(c => c.Assignments)
            .Include(c => c.Attendances)
            .Include(c => c.Institution)
            .Include(c => c.Grade)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (existClass != null)
            return new ClassesResponseDto() { Success = true, Class = existClass.Adapt<ClassDetailsDto>() };

        Log.Error("Class with id {Id} not found", id);

        return new ClassesResponseDto() { Success = false, Message = "Class not found" };
    }

    public async Task<ClassesResponseDto> AddClassAsync(UpsertClassDto classDto)
    {
        var validationResult = await validator.ValidateAsync(classDto);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));

            Log.Error(error);

            return new ClassesResponseDto() { Success = false, Message = error };
        }

        var newClass = classDto.Adapt<Class>();

        await context.Classes.AddAsync(newClass);
        await context.SaveChangesAsync();

        return new ClassesResponseDto()
        {
            Success = true, Message = "Class added successfully!",
            Class = newClass.Adapt<ClassDetailsDto>()
        };
    }

    public async Task<ClassesResponseDto> UpdateClassAsync(int id, UpsertClassDto classDto)
    {
        var existClass = await context.Classes.FindAsync(id);

        if (existClass == null)
        {
            Log.Error("Class with id {Id} not found", id);

            return new ClassesResponseDto() { Success = false, Message = "Class not found" };
        }

        var validationResult = await validator.ValidateAsync(classDto);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));

            Log.Error(error);

            return new ClassesResponseDto() { Success = false, Message = error };
        }

        existClass.Name = classDto.Name;
        existClass.Capacity = classDto.Capacity;
        existClass.GradeId = classDto.GradeId;
        existClass.InstitutionId = classDto.InstitutionId;
        existClass.TeacherId = classDto.TeacherId;

        await context.SaveChangesAsync();

        return new ClassesResponseDto() { Success = true, Message = "Class updated successfully!" };
    }

    public async Task<ClassesResponseDto> DeleteClassAsync(int id)
    {
        var existClass = await context.Classes.FindAsync(id);

        if (existClass == null)
        {
            Log.Error("Class with id {Id} not found", id);

            return new ClassesResponseDto() { Success = false, Message = "Class not found" };
        }

        context.Classes.Remove(existClass);
        await context.SaveChangesAsync();

        return new ClassesResponseDto() { Success = true, Message = "Class deleted successfully!" };
    }
}