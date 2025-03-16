using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SMS.Backend.Data;
using SMS.Models.DTOs.Parents;
using SMS.Models.Entities;

namespace SMS.Backend.Services.Parents;

public class ParentsService(
    AppDbContext context,
    IValidator<UpsertParentDto> validator) : IParentsService
{
    public async Task<ParentsResponseDto> GetParentsAsync()
    {
        var parents = await context.Parents.AsNoTracking()
            .OrderByDescending(p => p.CreatedAt)
            .ProjectToType<ParentDto>()
            .ToListAsync();

        return new ParentsResponseDto() { Success = true, Parents = parents };
    }

    public async Task<ParentsResponseDto> GetParentByIdAsync(int id)
    {
        var parent = await context.Parents.AsNoTracking()
            .Include(p => p.Students)
            .FirstOrDefaultAsync(p => p.Id == id);

        return parent == null
            ? new ParentsResponseDto() { Success = false, Message = "Parent not found" }
            : new ParentsResponseDto() { Success = true, Parent = parent.Adapt<ParentDetailsDto>() };
    }

    public async Task<ParentsResponseDto> AddParentAsync(UpsertParentDto parent)
    {
        var validationResult = await validator.ValidateAsync(parent);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(p => p.ErrorMessage));

            Log.Error(error);

            return new ParentsResponseDto() { Success = false, Message = error };
        }

        var newParent = parent.Adapt<Parent>();

        await context.Parents.AddAsync(newParent);
        await context.SaveChangesAsync();

        return new ParentsResponseDto()
        {
            Success = true,
            Message = "Parent added successfully!",
            Parent = newParent.Adapt<ParentDetailsDto>()
        };
    }

    public async Task<ParentsResponseDto> UpdateParentAsync(int id, UpsertParentDto parent)
    {
        var existingParent = await context.Parents.FindAsync(id);

        if (existingParent == null)
            return new ParentsResponseDto() { Success = false, Message = "Parent not found" };

        var validationResult = await validator.ValidateAsync(parent);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(p => p.ErrorMessage));

            Log.Error(error);

            return new ParentsResponseDto() { Success = false, Message = error };
        }

        existingParent.FullName = parent.FullName;
        existingParent.Email = parent.Email;
        existingParent.Phone = parent.Phone;
        existingParent.Address = parent.Address;

        await context.SaveChangesAsync();

        return new ParentsResponseDto() { Success = true, Message = "Parent updated successfully!" };
    }

    public async Task<ParentsResponseDto> DeleteParentAsync(int id)
    {
        var existingParent = await context.Parents.FindAsync(id);

        if (existingParent == null)
            return new ParentsResponseDto() { Success = false, Message = "Parent not found" };

        context.Parents.Remove(existingParent);
        await context.SaveChangesAsync();

        return new ParentsResponseDto() { Success = true, Message = "Parent deleted successfully!" };
    }
}