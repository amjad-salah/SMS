using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SMS.Backend.Data;
using SMS.Models.DTOs.ExamResults;
using SMS.Models.Entities;

namespace SMS.Backend.Services.ExamResults;

public class ExamResultsService(
    AppDbContext context,
    IValidator<UpsertExamResultDto> validator) : IExamResultsService
{
    public async Task<ExamResultsResponseDto> GetResultsAsync()
    {
        var results = await context.ExamResults.AsNoTracking()
            .Include(r => r.Exam)
            .Include(r => r.Student)
            .OrderByDescending(r => r.CreatedAt)
            .ProjectToType<ExamResultDto>()
            .ToListAsync();

        return new ExamResultsResponseDto() { Success = true, Results = results };
    }

    public async Task<ExamResultsResponseDto> GetResultByIdAsync(int id)
    {
        var result = await context.ExamResults.AsNoTracking()
            .Include(r => r.Exam)
            .Include(r => r.Student)
            .FirstOrDefaultAsync(r => r.Id == id);

        return result == null
            ? new ExamResultsResponseDto() { Success = true, Message = "Exam result not found" }
            : new ExamResultsResponseDto() { Success = true, Result = result.Adapt<ExamResultDto>() };
    }

    public async Task<ExamResultsResponseDto> GetResultByCurrentYearAsync()
    {
        var results = await context.ExamResults.AsNoTracking()
            .Include(r => r.Exam)
            .ThenInclude(e => e!.AcademicYear)
            .Include(r => r.Student)
            .Where(r => r.Exam != null &&
                        r.Exam.AcademicYear != null &&
                        r.Exam.AcademicYear.IsCurrent)
            .OrderByDescending(r => r.CreatedAt)
            .ProjectToType<ExamResultDto>()
            .ToListAsync();

        return new ExamResultsResponseDto() { Success = true, Results = results };
    }

    public async Task<ExamResultsResponseDto> AddResultAsync(UpsertExamResultDto result)
    {
        var validationResult = await validator.ValidateAsync(result);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

            Log.Error(error);

            return new ExamResultsResponseDto() { Success = false, Message = error };
        }

        var exam = await context.Exams.FindAsync(result.ExamId);

        // ReSharper disable once NullableWarningSuppressionIsUsed
        var percentage = decimal.Round(result.Score / exam!.MaxMark * 100, 2,
            MidpointRounding.AwayFromZero);


        result.Percentage = percentage;

        var newResult = result.Adapt<ExamResult>();

        await context.ExamResults.AddAsync(newResult);
        await context.SaveChangesAsync();

        return new ExamResultsResponseDto()
        {
            Success = true,
            Message = "Result added successfully!",
            Result = newResult.Adapt<ExamResultDto>()
        };
    }

    public async Task<ExamResultsResponseDto> UpdateResultAsync(int id, UpsertExamResultDto result)
    {
        var existResult = await context.ExamResults.FindAsync(id);

        if (existResult == null)
            return new ExamResultsResponseDto() { Success = false, Message = "Exam result not found" };

        var validationResult = await validator.ValidateAsync(result);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

            Log.Error(error);

            return new ExamResultsResponseDto() { Success = false, Message = error };
        }

        var exam = await context.Exams.FindAsync(result.ExamId);

        // ReSharper disable once NullableWarningSuppressionIsUsed
        var percentage = decimal.Round(result.Score / exam!.MaxMark * 100, 2,
            MidpointRounding.AwayFromZero);

        existResult.StudentId = result.StudentId;
        existResult.ExamId = result.ExamId;
        existResult.Score = result.Score;
        existResult.Percentage = percentage;

        await context.SaveChangesAsync();

        return new ExamResultsResponseDto() { Success = true, Message = "Result updated successfully!" };
    }

    public async Task<ExamResultsResponseDto> DeleteResultAsync(int id)
    {
        var existResult = await context.ExamResults.FindAsync(id);

        if (existResult == null)
            return new ExamResultsResponseDto() { Success = false, Message = "Exam result not found" };

        context.ExamResults.Remove(existResult);
        await context.SaveChangesAsync();

        return new ExamResultsResponseDto() { Success = true, Message = "Result deleted successfully!" };
    }
}