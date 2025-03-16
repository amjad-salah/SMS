using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SMS.Backend.Data;
using SMS.Models.DTOs.Institutions;
using SMS.Models.Entities;

namespace SMS.Backend.Services.Institutions;

public class InstitutionService(
    AppDbContext context,
    IValidator<UpsertInstitutionDto> validator) : IInstitutionService
{
    public async Task<InstitutionsResponseDto> GetInstitutionsAsync()
    {
        var institutions = await context.Institutions
            .AsNoTracking()
            .OrderByDescending(i => i.CreatedAt)
            .ProjectToType<InstitutionDto>()
            .ToListAsync();

        return new InstitutionsResponseDto() { Success = true, Institutions = institutions };
    }

    public async Task<InstitutionsResponseDto> GetInstitutionByIdAsync(int id)
    {
        var institution = await context.Institutions.AsNoTracking()
            .Include(i => i.Students)
            .Include(i => i.Teachers)
            .Include(i => i.Classes)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (institution == null)
        {
            Log.Error("Institution with id {Id} not found", id);
            return new InstitutionsResponseDto() { Success = false, Message = "Institution not found" };
        }


        return new InstitutionsResponseDto()
        {
            Success = true,
            Institution = institution.Adapt<InstitutionDetailsDto>()
        };
    }

    public async Task<InstitutionsResponseDto> AddInstitutionAsync(UpsertInstitutionDto institution)
    {
        var validationResult = await validator.ValidateAsync(institution);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));

            Log.Error(error);

            return new InstitutionsResponseDto() { Success = false, Message = error };
        }

        var exists = await context.Institutions.AnyAsync(i => i.Name == institution.Name);

        if (exists)
        {
            Log.Error("Institution with name {Name} already exists", institution.Name);

            return new InstitutionsResponseDto()
            {
                Success = false,
                Message = "Institution with this name already exists"
            };
        }

        var newInstitution = institution.Adapt<Institution>();

        context.Institutions.Add(newInstitution);
        await context.SaveChangesAsync();

        return new InstitutionsResponseDto()
        {
            Success = true,
            Institution = newInstitution.Adapt<InstitutionDetailsDto>(),
            Message = "Institution added successfully"
        };
    }

    public async Task<InstitutionsResponseDto> UpdateInstitutionAsync(int id, UpsertInstitutionDto institution)
    {
        var validationResult = await validator.ValidateAsync(institution);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));

            Log.Error(error);

            return new InstitutionsResponseDto() { Success = false, Message = error };
        }

        var existInstitution = await context.Institutions.FindAsync(id);

        if (existInstitution == null)
        {
            Log.Error("Institution with id {Id} not found", id);

            return new InstitutionsResponseDto() { Success = false, Message = "Institution not found" };
        }

        if (institution.Name != existInstitution.Name)
        {
            var exists = await context.Institutions.AnyAsync(i => i.Name == institution.Name);

            if (!exists)
            {
                Log.Error("Institution with name {Name} already exists", institution.Name);
                return new InstitutionsResponseDto()
                {
                    Success = false,
                    Message = "Institution with this name already exists"
                };
            }
        }

        existInstitution.Name = institution.Name;
        existInstitution.Email = institution.Email;
        existInstitution.Phone = institution.Phone;
        existInstitution.Address = institution.Address;
        existInstitution.InstitutionType = institution.InstitutionType;
        existInstitution.DateEstablished = institution.DateEstablished;

        await context.SaveChangesAsync();

        return new InstitutionsResponseDto()
        {
            Success = true,
            Message = "Institution updated successfully"
        };
    }

    public async Task<InstitutionsResponseDto> DeleteInstitutionAsync(int id)
    {
        var existInstitution = await context.Institutions.FindAsync(id);

        if (existInstitution == null)
        {
            Log.Error("Institution with id {Id} not found", id);

            return new InstitutionsResponseDto() { Success = false, Message = "Institution not found" };
        }

        context.Institutions.Remove(existInstitution);
        await context.SaveChangesAsync();

        return new InstitutionsResponseDto() { Success = true, Message = "Institution deleted successfully" };
    }
}