using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SMS.Backend.Data;
using SMS.Models.DTOs.Lessons;
using SMS.Models.Entities;

namespace SMS.Backend.Services.Lessons;

public class LessonsService(
    AppDbContext context,
    IValidator<UpsertLessonDto> validator) : ILessonsService
{
    public async Task<LessonsResponseDto> GetLessonsAsync()
    {
        var lessons = await context.Lessons.AsNoTracking()
            .Include(l => l.Class)
            .Include(l => l.Teacher)
            .Include(l => l.Subject)
            .OrderByDescending(l => l.CreatedAt)
            .ProjectToType<LessonDto>()
            .ToListAsync();

        return new LessonsResponseDto() { Success = true, Lessons = lessons };
    }

    public async Task<LessonsResponseDto> GetLessonByIdAsync(int id)
    {
        var lesson = await context.Lessons.AsNoTracking()
            .Include(l => l.Class)
            .Include(l => l.Teacher)
            .Include(l => l.Subject)
            .FirstOrDefaultAsync(l => l.Id == id);

        return lesson == null
            ? new LessonsResponseDto() { Success = false, Message = "Lesson not found" }
            : new LessonsResponseDto() { Success = true, Lesson = lesson.Adapt<LessonDto>() };
    }

    public async Task<LessonsResponseDto> AddLessonAsync(UpsertLessonDto lesson)
    {
        var validationResult = await validator.ValidateAsync(lesson);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

            Log.Error(error);

            return new LessonsResponseDto() { Success = false, Message = error };
        }

        var newLesson = lesson.Adapt<Lesson>();

        await context.Lessons.AddAsync(newLesson);
        await context.SaveChangesAsync();

        return new LessonsResponseDto()
        {
            Success = true,
            Message = "Lesson added successfully!",
            Lesson = newLesson.Adapt<LessonDto>()
        };
    }

    public async Task<LessonsResponseDto> UpdateLessonAsync(int id, UpsertLessonDto lesson)
    {
        var existLesson = await context.Lessons.FindAsync(id);

        if (existLesson == null)
            return new LessonsResponseDto() { Success = false, Message = "Lesson not found" };

        var validationResult = await validator.ValidateAsync(lesson);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

            Log.Error(error);

            return new LessonsResponseDto() { Success = false, Message = error };
        }

        existLesson.Day = lesson.Day;
        existLesson.StartTime = lesson.StartTime ?? TimeSpan.Zero;
        existLesson.EndTime = lesson.EndTime ??  TimeSpan.Zero;
        existLesson.SubjectId = lesson.SubjectId;
        existLesson.TeacherId = lesson.TeacherId;
        existLesson.ClassId = lesson.ClassId;

        await context.SaveChangesAsync();

        return new LessonsResponseDto()
        {
            Success = true,
            Message = "Lesson updated successfully!",
            Lesson = existLesson.Adapt<LessonDto>()
        };
    }

    public async Task<LessonsResponseDto> DeleteLessonAsync(int id)
    {
        var existLesson = await context.Lessons.FindAsync(id);

        if (existLesson == null)
            return new LessonsResponseDto() { Success = false, Message = "Lesson not found" };

        context.Lessons.Remove(existLesson);
        await context.SaveChangesAsync();

        return new LessonsResponseDto() { Success = true, Message = "Lesson deleted successfully!" };
    }
}