using SMS.Models.DTOs.Fees;

namespace SMS.Models.DTOs.Applications;

public class ApplicationApproveDto
{
    public int ClassId { get; set; }
    public decimal Tax { get; set; }
    public decimal Discount { get; set; }
    public List<FeeDto>? Fees { get; set; }
}