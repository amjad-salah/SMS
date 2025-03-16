using SMS.Models.Entities;

namespace SMS.Models.DTOs.Fees;

public class FeeDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public FeeType Type { get; set; }
    public string Name { get; set; } = string.Empty;
}