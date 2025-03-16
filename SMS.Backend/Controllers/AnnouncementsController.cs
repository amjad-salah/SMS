using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Backend.Services.Announcements;
using SMS.Backend.Utils;
using SMS.Models.DTOs.Announcements;

namespace SMS.Backend.Controllers;

[ApiController]
[Route("api/announcements")]
[Authorize]
public class AnnouncementsController(
    IAnnouncementsService service,
    AuthUtils authUtils) : ControllerBase
{
    //Get all announcements
    //GET /api/announcements
    [HttpGet("")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<AnnouncementResponseDto>> GetAnnouncements()
    {
        var response = await service.GetAnnouncementsAsync();

        return Ok(response);
    }

    //Get announcement by id
    //GET /api/announcements/:id
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AnnouncementResponseDto>> GetAnnouncement(int id)
    {
        var response = await service.GetAnnouncementByIdAsync(id);

        if (response.Success) return Ok(response);

        return NotFound(response);
    }

    //Add new announcement
    //POST /api/announcements
    [HttpPost("")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<ActionResult<AnnouncementResponseDto>> AddAnnouncement(UpsertAnnouncementDto dto)
    {
        var toke = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        var userId = authUtils.GetUserIdFromToken(toke);

        var response = await service.AddAnnouncementAsync(dto, userId);

        if (response.Success) return CreatedAtRoute("", response);

        return BadRequest(response);
    }

    //Update announcement by id
    //PUT /api/announcements/:id
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<ActionResult<AnnouncementResponseDto>> UpdateAnnouncement(int id, UpsertAnnouncementDto dto)
    {
        var toke = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var userId = authUtils.GetUserIdFromToken(toke);

        var response = await service.UpdateAnnouncementAsync(id, dto, userId);

        if (response.Success) return Ok(response);

        if (response.Message == "Announcement not found")
            return NotFound(response);

        return BadRequest(response);
    }

    //Delete announcement by id
    //DELETE /api/announcements/:id
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<ActionResult<AnnouncementResponseDto>> DeleteAnnouncement(int id)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var userId = authUtils.GetUserIdFromToken(token);

        var response = await service.DeleteAnnouncementAsync(id, userId);

        if (response.Success) return Ok(response);

        if (response.Message == "Announcement not found")
            return NotFound(response);

        return BadRequest(response);
    }
}