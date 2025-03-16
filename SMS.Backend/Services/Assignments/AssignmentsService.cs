using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SMS.Backend.Data;
using SMS.Models.DTOs.Assignments;
using SMS.Models.Entities;

namespace SMS.Backend.Services.Assignments;

public class AssignmentsService(
    AppDbContext context,
    IValidator<UpsertAssignmentDto> validator) : IAssignmentsService
{
    public async Task<AssignmentResponseDto> GetAssignmentsAsync()
    {
        var assignments = await context.Assignments.AsNoTracking()
            .Include(a => a.Class)
            .OrderByDescending(a => a.CreatedAt)
            .ProjectToType<AssignmentDto>()
            .ToListAsync();

        return new AssignmentResponseDto() { Success = true, Assignments = assignments };
    }

    public async Task<AssignmentResponseDto> GetAssignmentByIdAsync(int id)
    {
        var assignment = await context.Assignments.AsNoTracking()
            .Include(a => a.Class)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (assignment != null)
            return new AssignmentResponseDto() { Success = true, Assignment = assignment.Adapt<AssignmentDto>() };

        Log.Error("Assignment with id {id} could not be found", id);

        return new AssignmentResponseDto() { Success = true, Message = "Assignment not found" };
    }

    public async Task<AssignmentResponseDto> AddAssignmentAsync(UpsertAssignmentDto assignment, int userId)
    {
        var validationResult = await validator.ValidateAsync(assignment);

        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));

            Log.Error(errors);

            return new AssignmentResponseDto() { Success = false, Message = errors };
        }

        var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);

        // ReSharper disable once NullableWarningSuppressionIsUsed
        if (user!.Role == UserRole.Teacher)
        {
            var teacher = await context.Teachers.AsNoTracking().FirstOrDefaultAsync(t => t.UserId == userId);

            var anClass = await context.Classes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == assignment.ClassId);

            // ReSharper disable once NullableWarningSuppressionIsUsed
            if (teacher!.Id != anClass!.TeacherId)
            {
                Log.Error("Teacher is not class's teacher");
                return new AssignmentResponseDto() { Success = false, Message = "Teacher is not class's teacher" };
            }
        }

        var currentYear = await context.AcademicYears.AsNoTracking()
            .FirstOrDefaultAsync(a => a.IsCurrent);

        if (currentYear == null)
            return new AssignmentResponseDto() { Success = false, Message = "Current year doesn't exist" };

        var newAssignment = assignment.Adapt<Assignment>();

        newAssignment.AcademicYearId = currentYear.Id;

        await context.Assignments.AddAsync(newAssignment);
        await context.SaveChangesAsync();

        return new AssignmentResponseDto()
        {
            Success = true,
            Assignment = newAssignment.Adapt<AssignmentDto>(),
            Message = "Assignment added successfully!"
        };
    }

    public async Task<AssignmentResponseDto> UpdateAssignmentAsync(int id, UpsertAssignmentDto assignment)
    {
        var validationResult = await validator.ValidateAsync(assignment);

        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));

            Log.Error(errors);

            return new AssignmentResponseDto() { Success = false, Message = errors };
        }

        var existAssignment = await context.Assignments.FindAsync(id);

        if (existAssignment == null)
        {
            Log.Error("Assignment with id {id} could not be found", id);

            return new AssignmentResponseDto() { Success = false, Message = "Assignment not found" };
        }

        existAssignment.Title = assignment.Title;
        existAssignment.ClassId = assignment.ClassId;
        existAssignment.StartDate = assignment.StartDate;
        existAssignment.EndDate = assignment.EndDate;

        await context.SaveChangesAsync();

        return new AssignmentResponseDto() { Success = true, Message = "Assignment updated successfully!" };
    }

    public async Task<AssignmentResponseDto> DeleteAssignmentAsync(int id)
    {
        var existAssignment = await context.Assignments.FindAsync(id);

        if (existAssignment == null)
        {
            Log.Error("Assignment with id {id} could not be found", id);

            return new AssignmentResponseDto() { Success = false, Message = "Assignment not found" };
        }

        context.Assignments.Remove(existAssignment);
        await context.SaveChangesAsync();

        return new AssignmentResponseDto() { Success = true, Message = "Assignment deleted successfully!" };
    }
}