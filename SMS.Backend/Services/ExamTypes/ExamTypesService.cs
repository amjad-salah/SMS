using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SMS.Backend.Data;
using SMS.Models.DTOs.ExamTypes;
using SMS.Models.Entities;

namespace SMS.Backend.Services.ExamTypes;

public class ExamTypesService(
    AppDbContext context,
    IValidator<UpsertExamTypeDto> validator) : IExamTypesService
{
    public async Task<ExamTypesResponseDto> GetExamTypesAsync()
    {
        var types = await context.ExamTypes.AsNoTracking()
            .Include(t => t.Exams)
            .OrderByDescending(t => t.CreatedAt)
            .ProjectToType<ExamTypeDto>()
            .ToListAsync();

        return new ExamTypesResponseDto() { Success = true, ExamTypes = types };
    }

    public async Task<ExamTypesResponseDto> GetExamTypeByIdAsync(int id)
    {
        var type = await context.ExamTypes.AsNoTracking()
            .Include(t => t.Exams)
            .FirstOrDefaultAsync(t => t.Id == id);

        return type == null
            ? new ExamTypesResponseDto() { Success = false, Message = "Exam type not found" }
            : new ExamTypesResponseDto() { Success = true, ExamType = type.Adapt<ExamTypeDto>() };
    }

    public async Task<ExamTypesResponseDto> AddExamTypeAsync(UpsertExamTypeDto examType)
    {
        var validationResult = await validator.ValidateAsync(examType);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

            Log.Error(error);

            return new ExamTypesResponseDto() { Success = false, Message = error };
        }

        var newType = examType.Adapt<ExamType>();

        await context.ExamTypes.AddAsync(newType);
        await context.SaveChangesAsync();

        return new ExamTypesResponseDto()
        {
            Success = true,
            Message = "Exam type added successfully!",
            ExamType = newType.Adapt<ExamTypeDto>()
        };
    }

    public async Task<ExamTypesResponseDto> DeleteExamTypeAsync(int id)
    {
        var existingType = await context.ExamTypes.FindAsync(id);

        if (existingType == null)
            return new ExamTypesResponseDto() { Success = false, Message = "Exam type not found" };

        context.ExamTypes.Remove(existingType);
        await context.SaveChangesAsync();

        return new ExamTypesResponseDto() { Success = true, Message = "Exam type deleted successfully!" };
    }
}