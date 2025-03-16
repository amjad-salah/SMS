using SMS.Models.DTOs.Dashboards.Admin;
using SMS.Models.DTOs.Dashboards.Finance;
using SMS.Models.DTOs.Dashboards.Parent;
using SMS.Models.DTOs.Dashboards.Student;
using SMS.Models.DTOs.Dashboards.Support;
using SMS.Models.DTOs.Dashboards.Teacher;

namespace SMS.Backend.Services.Dashboard;

public interface IDashboardService
{
    Task<AdminDashboardDto> AdminDashboard();
    Task<StudentDashboardDto> StudentDashboard(int userId);
    Task<ParentDashboardDto> ParentDashboard(int userId);
    Task<TeacherDashboardDto> TeacherDashboard(int userId);
    Task<SupportDashboardDto> SupportDashboard();
    Task<FinanceDashboardDto> FinanceDashboard();
}