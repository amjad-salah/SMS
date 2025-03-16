using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Backend.Services.Attendances;
using SMS.Models.DTOs.Attendances;

namespace SMS.Backend.Controllers;

[ApiController]
[Route("api/attendances")]
[Authorize(Roles = "Admin,Teacher")]
public class AttendancesController(IAttendancesService service) : ControllerBase
{
    //Get all attendances
    //GET /api/attendances
    [HttpGet("")]
    public async Task<ActionResult<AttendanceResponseDto>> GetAttendances()
    {
        var response = await service.GetAttendancesAsync();

        return Ok(response);
    }

    //Update attendance present by id
    //GET /api/attendances/:id/presence
    [HttpPut("{id:int}/presence")]
    public async Task<ActionResult<AttendanceResponseDto>> UpdatePresence(int id)
    {
        var response = await service.UpdateAttendancePresentAsync(id);

        if (response.Message == "Attendance not found")
            return NotFound(response);

        return Ok(response);
    }
}