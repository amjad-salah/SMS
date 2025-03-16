using Mapster;
using Microsoft.EntityFrameworkCore;
using SMS.Backend.Data;
using SMS.Models.DTOs.Classes;
using SMS.Models.DTOs.Dashboards.Admin;
using SMS.Models.DTOs.Dashboards.Finance;
using SMS.Models.DTOs.Dashboards.Parent;
using SMS.Models.DTOs.Dashboards.Student;
using SMS.Models.DTOs.Dashboards.Support;
using SMS.Models.DTOs.Dashboards.Teacher;
using SMS.Models.DTOs.Lessons;
using SMS.Models.DTOs.Students;
using SMS.Models.DTOs.Teachers;
using SMS.Models.Entities;

namespace SMS.Backend.Services.Dashboard;

public class DashboardService(AppDbContext context) : IDashboardService
{
    public async Task<AdminDashboardDto> AdminDashboard()
    {
        var studentsCount = await context.Students.AsNoTracking().CountAsync();
        var teachersCount = await context.Teachers.AsNoTracking().CountAsync();
        var activeStudentsCount =
            await context.Students.AsNoTracking().CountAsync(s => s.Status == StudentStatus.Active);
        var graduatedStudentsCount =
            await context.Students.AsNoTracking().CountAsync(s => s.Status == StudentStatus.Graduated);
        var pendingInvoicesCount =
            await context.Invoices.AsNoTracking().CountAsync(i => i.Status == InvoiceStatus.Pending);
        var paidInvoicesCount = await context.Invoices.AsNoTracking().CountAsync(i => i.Status == InvoiceStatus.Paid);
        var paidAmount = await context.Invoices.AsNoTracking().SumAsync(i => i.PaidAmount);
        var remainingAmount = await context.Invoices.AsNoTracking().SumAsync(i => i.RemainingBalance);
        var applicationsCount = await context.Applications.AsNoTracking().CountAsync();
        var approvedAppCount = await context.Applications.AsNoTracking()
            .CountAsync(a => a.Status == ApplicationStatus.Approved);

        var applicationByYear = await context.Applications.AsNoTracking()
            .Include(a => a.AcademicYear)
            .GroupBy(a => a.AcademicYear)
            .Select(g =>
                new ApplicationByYearDto { YearName = g.Key!.Name, ApplicationNo = g.Count() })
            .ToListAsync();

        return new AdminDashboardDto()
        {
            Success = true,
            StudentsCount = studentsCount,
            ApprovedAppCount = approvedAppCount,
            ApplicationsCount = applicationsCount,
            ApplicationByYear = applicationByYear,
            TeachersCount = teachersCount,
            Graduated = graduatedStudentsCount,
            PaidAmount = paidAmount,
            RemainingAmount = remainingAmount,
            ActiveStudentsCount = activeStudentsCount,
            PaidInvoices = paidInvoicesCount,
            PendingInvoices = pendingInvoicesCount
        };
    }

    public async Task<StudentDashboardDto> StudentDashboard(int userId)
    {
        var user = await context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return new StudentDashboardDto() { Success = false, Message = "User not found" };

        var student = await context.Students.AsNoTracking()
            .Include(s => s.Class)
            .Include(s => s.Invoices)
            .Include(s => s.Grade)
            .Include(s => s.Attendances)
            .Include(s => s.ExamResults)
            .Include(s => s.Records)
            .Include(s => s.Parent)
            .Include(s => s.Institution)
            .ProjectToType<StudentDetailsDto>()
            .FirstOrDefaultAsync(s => s.UserId == user.Id);

        if (student == null)
            return new StudentDashboardDto() { Success = false, Message = "Student not found" };

        var studentClass = await context.Classes.AsNoTracking()
            .Include(c => c.Assignments)
            .Include(c => c.Announcements)
            .Include(c => c.Lessons)
            .Include(c => c.Teacher)
            .FirstOrDefaultAsync(c => c.Id == student.ClassId);

        if (studentClass == null)
            return new StudentDashboardDto() { Success = false, Message = "Class not found" };


        return new StudentDashboardDto()
        {
            Success = true, Student = student,
            Class = studentClass.Adapt<ClassDetailsDto>()
        };
    }

    public async Task<ParentDashboardDto> ParentDashboard(int userId)
    {
        var user = await context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return new ParentDashboardDto() { Success = false, Message = "User not found" };

        var parent = await context.Parents.AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == user.Id);

        if (parent == null)
            return new ParentDashboardDto() { Success = false, Message = "Parent not found" };

        var students = await context.Students.AsNoTracking()
            .Include(s => s.Class)
            .Include(s => s.Grade)
            .Include(s => s.AcademicYear)
            .ProjectToType<StudentDto>()
            .ToListAsync();

        var classes = new List<ClassDetailsDto>();

        foreach (var student in students)
        {
            var studentClass = await context.Classes.AsNoTracking()
                .Include(c => c.Assignments)
                .Include(c => c.Announcements)
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(c => c.Id == student.ClassId);

            if (studentClass == null)
                continue;
            classes.Add(studentClass.Adapt<ClassDetailsDto>());
        }

        return new ParentDashboardDto()
        {
            Success = true, Students = students,
            Classes = classes
        };
    }

    public async Task<TeacherDashboardDto> TeacherDashboard(int userId)
    {
        var user = await context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return new TeacherDashboardDto() { Success = false, Message = "User not found" };

        var teacher = await context.Teachers.AsNoTracking()
            .Include(t => t.Institution)
            .Include(t => t.Lessons)
            .FirstOrDefaultAsync(t => t.UserId == user.Id);

        if (teacher == null)
            return new TeacherDashboardDto() { Success = false, Message = "Teacher not found" };

        var teacherClasses = await context.Classes.AsNoTracking()
            .Where(c => c.TeacherId == teacher.Id)
            .Include(c => c.Assignments)
            .Include(c => c.Announcements)
            .Include(c => c.Attendances)
            .Include(c => c.Grade)
            .ProjectToType<ClassDetailsDto>()
            .ToListAsync();

        return new TeacherDashboardDto()
        {
            Success = true, Teacher = teacher.Adapt<TeacherDetailsDto>(),
            Classes = teacherClasses
        };
    }

    public async Task<SupportDashboardDto> SupportDashboard()
    {
        var currentYear = await context.AcademicYears.AsNoTracking()
            .FirstOrDefaultAsync(a => a.IsCurrent);

        if (currentYear == null)
            return new SupportDashboardDto() { Success = false, Message = "No current academic year found" };

        var currentStudentsCount = await context.Students.AsNoTracking()
            .CountAsync(s => s.AcademicYearId == currentYear.Id);

        var currentActiveStudentsCount = await context.Students.AsNoTracking()
            .CountAsync(s => s.AcademicYearId == currentYear.Id && s.Status == StudentStatus.Active);

        var currentApplicationsCount = await context.Applications.AsNoTracking()
            .CountAsync(s => s.AcademicYearId == currentYear.Id);

        var currentApprovedAppCount = await context.Applications.AsNoTracking()
            .CountAsync(s => s.AcademicYearId == currentYear.Id && s.Status == ApplicationStatus.Approved);

        var classesCount = await context.Classes.AsNoTracking().CountAsync();

        var todayLessons = await context.Lessons.AsNoTracking()
            .Where(s => s.Day == DateTime.Now.DayOfWeek)
            .ProjectToType<LessonDto>()
            .ToListAsync();

        return new SupportDashboardDto()
        {
            Success = true, ClassesCount = classesCount, TodayLessons = todayLessons,
            CurrentApplicationsCount = currentApplicationsCount, CurrentApprovedAppCount = currentApprovedAppCount,
            CurrentStudentsCount = currentStudentsCount, CurrentActiveStudentsCount = currentActiveStudentsCount
        };
    }

    public async Task<FinanceDashboardDto> FinanceDashboard()
    {
        var pendingInvoicesCount =
            await context.Invoices.AsNoTracking().CountAsync(i => i.Status == InvoiceStatus.Pending);
        var paidInvoicesCount = await context.Invoices.AsNoTracking().CountAsync(i => i.Status == InvoiceStatus.Paid);
        var paidAmount = await context.Invoices.AsNoTracking().SumAsync(i => i.PaidAmount);
        var remainingAmount = await context.Invoices.AsNoTracking().SumAsync(i => i.RemainingBalance);

        var monthlyRevenue = await context.Invoices.AsNoTracking()
            .Where(i => i.Date.Year == DateTime.Now.Year)
            .GroupBy(i => i.Date.Month)
            .Select(g => new MonthlyRevenueDto()
            {
                Month = g.Key.ToString(),
                Amount = g.Sum(i => i.PaidAmount)
            })
            .ToListAsync();

        var yearlyRevenue = await context.Invoices.AsNoTracking()
            .GroupBy(i => i.Date.Year)
            .Select(g => new YearlyRevenueDto()
            {
                Year = g.Key.ToString(),
                Amount = g.Sum(i => i.PaidAmount)
            })
            .ToListAsync();

        return new FinanceDashboardDto()
        {
            Success = true, MonthlyRevenue = monthlyRevenue,
            YearlyRevenue = yearlyRevenue, PaidAmount = paidAmount,
            RemainingAmount = remainingAmount, PaidInvoices = paidInvoicesCount,
            PendingInvoices = pendingInvoicesCount
        };
    }
}