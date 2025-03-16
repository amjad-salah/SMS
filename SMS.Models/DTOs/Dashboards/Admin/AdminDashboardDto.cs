namespace SMS.Models.DTOs.Dashboards.Admin;

public class AdminDashboardDto : BaseResponseDto
{
    public int StudentsCount { get; set; }
    public int TeachersCount { get; set; }
    public int ActiveStudentsCount { get; set; }
    public int Graduated { get; set; }
    public int PendingInvoices { get; set; }
    public int PaidInvoices { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public int ApplicationsCount { get; set; }
    public int ApprovedAppCount { get; set; }
    public List<ApplicationByYearDto> ApplicationByYear { get; set; } = new();
}