using SMS.Models.Entities;

namespace SMS.Models.DTOs.Fees;

public class UpsertFeeDto
{
    public decimal Amount { get; set; }
    public FeeType Type { get; set; }
    public string Name { get; set; } = string.Empty;
}