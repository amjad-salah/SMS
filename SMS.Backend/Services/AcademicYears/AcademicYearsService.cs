using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SMS.Backend.Data;
using SMS.Models.DTOs.AcademicYears;
using SMS.Models.Entities;

namespace SMS.Backend.Services.AcademicYears;

public class AcademicYearsService(
    AppDbContext context,
    IValidator<UpsertAcademicYearDto> validator) : IAcademicYearsService
{
    public async Task<AcademicYearsResponseDto> GetAcademicYearsAsync()
    {
        var academicYears = await context.AcademicYears.AsNoTracking()
            .OrderByDescending(a => a.CreatedAt)
            .ProjectToType<AcademicYearDto>()
            .ToListAsync();

        return new AcademicYearsResponseDto() { Success = true, AcademicYears = academicYears };
    }

    public async Task<AcademicYearsResponseDto> GetAcademicYearByIdAsync(int id)
    {
        var academicYear = await context.AcademicYears.AsNoTracking()
            .Include(a => a.Exams)
            .Include(a => a.Students)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (academicYear != null)
            return new AcademicYearsResponseDto()
            {
                Success = true,
                AcademicYear = academicYear.Adapt<AcademicYearDetailsDto>()
            };

        Log.Error("Academic Year with id {Id} not found", id);

        return new AcademicYearsResponseDto() { Success = false, Message = "Year not found!" };
    }

    public async Task<AcademicYearsResponseDto> AddAcademicYearAsync(UpsertAcademicYearDto year)
    {
        var validationResult = await validator.ValidateAsync(year);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

            Log.Error(error);

            return new AcademicYearsResponseDto() { Success = false, Message = error };
        }

        if (context.AcademicYears.Any(a => a.IsCurrent) && year.IsCurrent)
        {
            Log.Error("Current academic year is already exist");

            return new AcademicYearsResponseDto()
            {
                Success = false,
                Message = "Current academic year is already exist"
            };
        }

        var newYear = year.Adapt<AcademicYear>();
        await context.AcademicYears.AddAsync(newYear);
        await context.SaveChangesAsync();

        return new AcademicYearsResponseDto()
        {
            Success = true,
            AcademicYear = newYear.Adapt<AcademicYearDetailsDto>(),
            Message = "Year added successfully!"
        };
    }

    public async Task<AcademicYearsResponseDto> UpdateAcademicYearAsync(int id, UpsertAcademicYearDto year)
    {
        var validationResult = await validator.ValidateAsync(year);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

            Log.Error(error);

            return new AcademicYearsResponseDto() { Success = false, Message = error };
        }

        var existYear = await context.AcademicYears.FindAsync(id);

        if (existYear == null)
        {
            Log.Error("Academic Year with id {Id} not found", id);
            return new AcademicYearsResponseDto() { Success = false, Message = "Year not found!" };
        }

        if (context.AcademicYears.Any(a => a.IsCurrent) && year.IsCurrent)
        {
            Log.Error("Current academic year is already exist");

            return new AcademicYearsResponseDto()
            {
                Success = false,
                Message = "Current academic year is already exist"
            };
        }

        existYear.Name = year.Name;
        existYear.StartDate = year.StartDate;
        existYear.EndDate = year.EndDate;
        existYear.IsCurrent = year.IsCurrent;

        await context.SaveChangesAsync();

        return new AcademicYearsResponseDto()
        {
            Success = true,
            Message = "Year updated successfully!"
        };
    }

    public async Task<AcademicYearsResponseDto> DeleteAcademicYearAsync(int id)
    {
        var existYear = await context.AcademicYears.FindAsync(id);

        if (existYear == null)
        {
            Log.Error("Academic Year with id {Id} not found", id);
            return new AcademicYearsResponseDto() { Success = false, Message = "Year not found!" };
        }

        context.AcademicYears.Remove(existYear);
        await context.SaveChangesAsync();

        return new AcademicYearsResponseDto()
        {
            Success = true,
            Message = "Year deleted successfully!"
        };
    }
}