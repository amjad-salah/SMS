using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Backend.Services.Dashboard;
using SMS.Backend.Utils;
using SMS.Models.DTOs.Dashboards.Admin;
using SMS.Models.DTOs.Dashboards.Finance;
using SMS.Models.DTOs.Dashboards.Parent;
using SMS.Models.DTOs.Dashboards.Student;
using SMS.Models.DTOs.Dashboards.Support;
using SMS.Models.DTOs.Dashboards.Teacher;

namespace SMS.Backend.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController(
    IDashboardService service,
    AuthUtils authUtils) : ControllerBase
{
    //Admin dashboard
    //GET /api/dashboard/admin
    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<AdminDashboardDto>> AdminDashboard()
    {
        var response = await service.AdminDashboard();

        if (!response.Success) return BadRequest(response);

        return Ok(response);
    }

    //Student dashboard
    //GET /api/dashboard/student
    [HttpGet("student")]
    [Authorize(Roles = "Student")]
    public async Task<ActionResult<StudentDashboardDto>> StudentDashboard()
    {
        var toke = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        var userId = authUtils.GetUserIdFromToken(toke);

        var response = await service.StudentDashboard(userId);

        if (!response.Success) return BadRequest(response);

        return Ok(response);
    }

    //Parent dashboard
    //GET /api/dashboard/parent
    [HttpGet("parent")]
    [Authorize(Roles = "Parent")]
    public async Task<ActionResult<ParentDashboardDto>> ParentDashboard()
    {
        var toke = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        var userId = authUtils.GetUserIdFromToken(toke);

        var response = await service.ParentDashboard(userId);

        if (!response.Success) return BadRequest(response);

        return Ok(response);
    }

    //Teacher dashboard
    //GET /api/dashboard/teacher
    [HttpGet("teacher")]
    [Authorize(Roles = "Teacher")]
    public async Task<ActionResult<TeacherDashboardDto>> TeacherDashboard()
    {
        var toke = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        var userId = authUtils.GetUserIdFromToken(toke);

        var response = await service.TeacherDashboard(userId);

        if (!response.Success) return BadRequest(response);

        return Ok(response);
    }

    //Support dashboard
    //GET /api/dashboard/support
    [HttpGet("support")]
    [Authorize(Roles = "Support,Registrar")]
    public async Task<ActionResult<SupportDashboardDto>> SupportDashboard()
    {
        var response = await service.SupportDashboard();

        if (!response.Success) return BadRequest(response);

        return Ok(response);
    }

    //Finance dashboard
    //GET /api/dashboard/finance
    [HttpGet("finance")]
    [Authorize(Roles = "Accountant,Admin")]
    public async Task<ActionResult<FinanceDashboardDto>> FinanceDashboard()
    {
        var response = await service.FinanceDashboard();

        if (!response.Success) return BadRequest(response);

        return Ok(response);
    }
}