using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SMS.Backend.Data;
using SMS.Models.DTOs.Subjects;
using SMS.Models.Entities;

namespace SMS.Backend.Services.Subjects;

public class SubjectsService(
    AppDbContext context,
    IValidator<UpsertSubjectDto> validator) : ISubjectsService
{
    public async Task<SubjectsResponseDto> GetSubjectsAsync()
    {
        var subjects = await context.Subjects.AsNoTracking()
            .OrderByDescending(s => s.CreatedAt)
            .ProjectToType<SubjectDto>()
            .ToListAsync();

        return new SubjectsResponseDto() { Success = true, Subjects = subjects };
    }

    public async Task<SubjectsResponseDto> GetSubjectByIdAsync(int id)
    {
        var subject = await context.Subjects.AsNoTracking()
            .Include(s => s.Grade)
            .Include(s => s.Lessons)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (subject == null)
            return new SubjectsResponseDto() { Success = true, Message = "Subject not found" };

        return new SubjectsResponseDto() { Success = true, Subject = subject.Adapt<SubjectDetailsDto>() };
    }

    public async Task<SubjectsResponseDto> AddSubjectAsync(UpsertSubjectDto subject)
    {
        var validationResult = await validator.ValidateAsync(subject);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

            Log.Error(error);

            return new SubjectsResponseDto() { Success = false, Message = error };
        }

        var newSubject = subject.Adapt<Subject>();

        await context.Subjects.AddAsync(newSubject);
        await context.SaveChangesAsync();

        return new SubjectsResponseDto()
        {
            Success = true,
            Message = "Subject added Successfully!",
            Subject = newSubject.Adapt<SubjectDetailsDto>()
        };
    }

    public async Task<SubjectsResponseDto> UpdateSubjectAsync(int id, UpsertSubjectDto subject)
    {
        var existingSubject = await context.Subjects.FindAsync(id);

        if (existingSubject == null)
            return new SubjectsResponseDto() { Success = false, Message = "Subject not found" };

        var validationResult = await validator.ValidateAsync(subject);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

            Log.Error(error);

            return new SubjectsResponseDto() { Success = false, Message = error };
        }

        existingSubject.Name = subject.Name;
        existingSubject.GradeId = subject.GradeId;

        await context.SaveChangesAsync();

        return new SubjectsResponseDto() { Success = true, Message = "Subject updated successfully!" };
    }

    public async Task<SubjectsResponseDto> DeleteSubjectAsync(int id)
    {
        var existingSubject = await context.Subjects.FindAsync(id);

        if (existingSubject == null)
            return new SubjectsResponseDto() { Success = false, Message = "Subject not found" };

        context.Subjects.Remove(existingSubject);
        await context.SaveChangesAsync();

        return new SubjectsResponseDto() { Success = true, Message = "Subject deleted successfully!" };
    }
}