using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SMS.Backend.Data;
using SMS.Models.DTOs.Fees;
using SMS.Models.Entities;

namespace SMS.Backend.Services.Fees;

public class FeesService(
    AppDbContext context,
    IValidator<UpsertFeeDto> validator) : IFeesService
{
    public async Task<FeesResponseDto> GetFeesAsync()
    {
        var fees = await context.Fees.AsNoTracking()
            .OrderByDescending(f => f.CreatedAt)
            .ProjectToType<FeeDto>()
            .ToListAsync();

        return new FeesResponseDto() { Success = true, Fees = fees };
    }

    public async Task<FeesResponseDto> GetFeesByIdAsync(int id)
    {
        var fee = await context.Fees.AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == id);

        return fee == null
            ? new FeesResponseDto() { Success = false, Message = "Fee not found" }
            : new FeesResponseDto() { Success = true, Fee = fee.Adapt<FeeDto>() };
    }

    public async Task<FeesResponseDto> AddFeeAsync(UpsertFeeDto fee)
    {
        var validationResult = await validator.ValidateAsync(fee);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

            Log.Error(error);

            return new FeesResponseDto() { Success = false, Message = error };
        }

        var newFee = fee.Adapt<Fee>();

        await context.Fees.AddAsync(newFee);
        await context.SaveChangesAsync();

        return new FeesResponseDto()
        {
            Success = true,
            Message = "Fee added successfully!",
            Fee = newFee.Adapt<FeeDto>()
        };
    }

    public async Task<FeesResponseDto> DeleteFeeAsync(int id)
    {
        var fee = await context.Fees.FindAsync(id);

        if (fee == null)
            return new FeesResponseDto() { Success = false, Message = "Fee not found" };

        context.Fees.Remove(fee);
        await context.SaveChangesAsync();

        return new FeesResponseDto() { Success = true, Message = "Fee deleted successfully!" };
    }
}