using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SMS.Backend.Data;
using SMS.Models.DTOs.Attendances;
using SMS.Models.Entities;

namespace SMS.Backend.Services.Attendances;

public class AttendancesService(
    AppDbContext context,
    IValidator<UpsertAttendanceDto> validator) : IAttendancesService
{
    public async Task<AttendanceResponseDto> GetAttendancesAsync()
    {
        var attendances = await context.Attendances.AsNoTracking()
            .Include(a => a.Student)
            .Include(a => a.Class)
            .OrderByDescending(a => a.CreatedAt)
            .ProjectToType<AttendanceDto>()
            .ToListAsync();

        return new AttendanceResponseDto() { Success = true, Attendances = attendances };
    }

    public async Task<AttendanceResponseDto> AddAttendanceAsync(UpsertAttendanceDto attendance)
    {
        var validationResult = await validator.ValidateAsync(attendance);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));

            Log.Error(error);

            return new AttendanceResponseDto() { Success = false, Message = error };
        }

        var newAttendance = attendance.Adapt<Attendance>();

        await context.Attendances.AddAsync(newAttendance);
        await context.SaveChangesAsync();

        return new AttendanceResponseDto() { Success = true, Message = "Attendance added successfully" };
    }

    public async Task<AttendanceResponseDto> UpdateAttendancePresentAsync(int id)
    {
        var attendance = await context.Attendances.FindAsync(id);

        if (attendance == null)
        {
            Log.Error("Attendance with id {Id} not found", id);
            return new AttendanceResponseDto() { Success = false, Message = "Attendance not found" };
        }

        attendance.Present = true;
        await context.SaveChangesAsync();

        return new AttendanceResponseDto() { Success = true, Message = "Attendance updated successfully" };
    }

    public async Task<AttendanceResponseDto> AddClassAttendances(AddClassAttendancesDto request)
    {
        var existClass = await context.Classes.AsNoTracking()
            .Include(c => c.Students)
            .AsNoTracking().FirstOrDefaultAsync(c => c.Id == request.ClassId);

        if (existClass == null)
        {
            Log.Error("Class with id {ClassId} not found", request.ClassId);
            return new AttendanceResponseDto() { Success = false, Message = "Class not found" };
        }

        request.Date ??= DateTime.Now;

        if (existClass.Students != null)
        {
            foreach (var attendance in existClass.Students.Select(student => new UpsertAttendanceDto()
                     {
                         StudentId = student.Id,
                         ClassId = request.ClassId,
                         Date = request.Date ?? DateTime.Now,
                         Present = false
                     }))
                await AddAttendanceAsync(attendance);

            return new AttendanceResponseDto() { Success = true, Message = "Attendances added successfully" };
        }

        Log.Error("Failed to add class attendances");
        return new AttendanceResponseDto() { Success = false, Message = "Failed to add class attendances" };
    }
}