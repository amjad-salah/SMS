using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SMS.Backend.Data;
using SMS.Models.DTOs.Exams;
using SMS.Models.Entities;

namespace SMS.Backend.Services.Exams;

public class ExamsService(
    AppDbContext context,
    IValidator<UpsertExamDto> validator) : IExamsService
{
    public async Task<ExamsResponseDto> GetExamsAsync()
    {
        var exams = await context.Exams.AsNoTracking()
            .Include(e => e.AcademicYear)
            .Include(e => e.Grade)
            .Include(e => e.ExamType)
            .OrderByDescending(e => e.CreatedAt)
            .ProjectToType<ExamDto>()
            .ToListAsync();

        return new ExamsResponseDto() { Success = true, Exams = exams };
    }

    public async Task<ExamsResponseDto> GetExamByIdAsync(int id)
    {
        var exam = await context.Exams.AsNoTracking()
            .Include(e => e.AcademicYear)
            .Include(e => e.Grade)
            .Include(e => e.ExamType)
            .Include(e => e.Results)
            .FirstOrDefaultAsync(e => e.Id == id);

        return exam == null
            ? new ExamsResponseDto() { Success = false, Message = "Exam not found" }
            : new ExamsResponseDto() { Success = true, Exam = exam.Adapt<ExamDetailsDto>() };
    }

    public async Task<ExamsResponseDto> ApproveExamResultAsync(int id)
    {
        var exam = await context.Exams.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);

        if (exam == null)
            return new ExamsResponseDto() { Success = false, Message = "Exam not found" };

        var results = await context.ExamResults
            .Where(r => r.ExamId == exam.Id).ToListAsync();

        foreach (var result in results)
        {
            result.Approved = true;
            await context.SaveChangesAsync();
        }

        return new ExamsResponseDto() { Success = true, Message = "Exam approved" };
    }

    public async Task<ExamsResponseDto> GetExamsByCurrentYearAsync()
    {
        var exams = await context.Exams.AsNoTracking()
            .Include(e => e.AcademicYear)
            .Include(e => e.Grade)
            .Include(e => e.ExamType)
            .Where(e => e.AcademicYear != null && e.AcademicYear.IsCurrent)
            .OrderByDescending(e => e.CreatedAt)
            .ProjectToType<ExamDto>()
            .ToListAsync();

        return new ExamsResponseDto() { Success = true, Exams = exams };
    }

    public async Task<ExamsResponseDto> AddExamAsync(UpsertExamDto exam)
    {
        var validationResult = await validator.ValidateAsync(exam);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

            Log.Error(error);

            return new ExamsResponseDto() { Success = false, Message = error };
        }

        var newExam = exam.Adapt<Exam>();

        await context.Exams.AddAsync(newExam);
        await context.SaveChangesAsync();

        return new ExamsResponseDto()
        {
            Success = true,
            Message = "Exam added successfully!",
            Exam = newExam.Adapt<ExamDetailsDto>()
        };
    }

    public async Task<ExamsResponseDto> UpdateExamAsync(int id, UpsertExamDto exam)
    {
        var existExam = await context.Exams.FindAsync(id);

        if (existExam == null) return new ExamsResponseDto() { Success = false, Message = "Exam not found" };

        var validationResult = await validator.ValidateAsync(exam);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

            Log.Error(error);

            return new ExamsResponseDto() { Success = false, Message = error };
        }

        existExam.ExamTypeId = exam.ExamTypeId;
        existExam.GradeId = exam.GradeId;
        existExam.AcademicYearId = exam.AcademicYearId;
        existExam.ExamDate = exam.ExamDate;
        existExam.StartTime = exam.StartTime;
        existExam.EndTime = exam.EndTime;
        existExam.MinMark = exam.MinMark;
        existExam.MaxMark = exam.MaxMark;

        await context.SaveChangesAsync();

        return new ExamsResponseDto() { Success = true, Message = "Exam updated successfully!" };
    }

    public async Task<ExamsResponseDto> DeleteExamAsync(int id)
    {
        var existExam = await context.Exams.FindAsync(id);

        if (existExam == null) return new ExamsResponseDto() { Success = false, Message = "Exam not found" };

        context.Exams.Remove(existExam);
        await context.SaveChangesAsync();

        return new ExamsResponseDto() { Success = true, Message = "Exam deleted successfully!" };
    }
}