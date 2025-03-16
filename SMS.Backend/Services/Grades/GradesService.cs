using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SMS.Backend.Data;
using SMS.Models.DTOs.Grades;
using SMS.Models.Entities;

namespace SMS.Backend.Services.Grades;

public class GradesService(
    AppDbContext context,
    IValidator<UpsertGradeDto> validator) : IGradesService
{
    public async Task<GradesResponseDto> GetGradesAsync()
    {
        var grades = await context.Grades.AsNoTracking()
            .OrderByDescending(g => g.CreatedAt)
            .ProjectToType<GradeDto>()
            .ToListAsync();

        return new GradesResponseDto { Success = true, Grades = grades };
    }

    public async Task<GradesResponseDto> GetGradeByIdAsync(int id)
    {
        var grade = await context.Grades.AsNoTracking()
            .Include(g => g.Classes)
            .Include(g => g.Students)
            .Include(g => g.Exams)
            .Include(g => g.Subjects)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (grade == null)
            return new GradesResponseDto() { Success = false, Message = "Grade not found" };

        return new GradesResponseDto { Success = true, Grade = grade.Adapt<GradeDetailsDto>() };
    }

    public async Task<GradesResponseDto> AddGradeAsync(UpsertGradeDto grade)
    {
        var validationResult = await validator.ValidateAsync(grade);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

            Log.Error(error);

            return new GradesResponseDto { Success = false, Message = error };
        }

        var newGrade = grade.Adapt<Grade>();

        await context.Grades.AddAsync(newGrade);
        await context.SaveChangesAsync();

        return new GradesResponseDto
        {
            Success = true,
            Message = "Grade added successfully!",
            Grade = newGrade.Adapt<GradeDetailsDto>()
        };
    }

    public async Task<GradesResponseDto> UpdateGradeAsync(int id, UpsertGradeDto grade)
    {
        var existingGrade = await context.Grades.FindAsync(id);

        if (existingGrade == null) return new GradesResponseDto { Success = false, Message = "Grade not found" };

        var validationResult = await validator.ValidateAsync(grade);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

            Log.Error(error);

            return new GradesResponseDto { Success = false, Message = error };
        }

        existingGrade.Name = grade.Name;

        await context.SaveChangesAsync();

        return new GradesResponseDto() { Success = true, Message = "Grade updated successfully!" };
    }

    public async Task<GradesResponseDto> DeleteGradeAsync(int id)
    {
        var existingGrade = await context.Grades.FindAsync(id);

        if (existingGrade == null) return new GradesResponseDto { Success = false, Message = "Grade not found" };

        context.Grades.Remove(existingGrade);
        await context.SaveChangesAsync();

        return new GradesResponseDto { Success = true, Message = "Grade deleted successfully!" };
    }
}