using SMS.Models.DTOs.Dashboards.Finance;
using SMS.Models.DTOs.Lessons;

namespace SMS.Models.DTOs.Dashboards.Support;

public class SupportDashboardDto : FinanceDashboardDto
{
    public int CurrentStudentsCount { get; set; }
    public int CurrentActiveStudentsCount { get; set; }
    public int CurrentApplicationsCount { get; set; }
    public int CurrentApprovedAppCount { get; set; }
    public int ClassesCount { get; set; }
    public List<LessonDto> TodayLessons { get; set; } = [];
}