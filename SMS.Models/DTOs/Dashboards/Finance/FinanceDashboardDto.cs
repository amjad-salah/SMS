namespace SMS.Models.DTOs.Dashboards.Finance;

public class FinanceDashboardDto : BaseResponseDto
{
    public int PendingInvoices { get; set; }
    public int PaidInvoices { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public List<MonthlyRevenueDto> MonthlyRevenue { get; set; } = new();
    public List<YearlyRevenueDto> YearlyRevenue { get; set; } = new();
}