using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Backend.Services.Teachers;
using SMS.Backend.Utils;
using SMS.Models.DTOs.Teachers;

namespace SMS.Backend.Controllers;

[ApiController]
[Route("api/teachers")]
[Authorize(Roles = "Admin,Support,Registrar,Teacher")]
public class TeachersController(
    ITeachersService service,
    AuthUtils authUtils) : ControllerBase
{
    //Get all teachers
    //GET /api/teachers
    [HttpGet("")]
    public async Task<ActionResult<TeachersResponseDto>> GetTeachers()
    {
        var response = await service.GetTeachersAsync();

        return Ok(response);
    }

    //Get teacher by id
    //GET /api/teachers/:id
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TeachersResponseDto>> GetTeacher(int id)
    {
        var toke = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        var userId = authUtils.GetUserIdFromToken(toke);

        var response = await service.GetTeacherByIdAsync(id, userId);

        if (response.Success) return Ok(response);

        if (response.Message == "Teacher not found") return NotFound(response);

        return BadRequest(response);
    }

    //Add new teacher
    //POST /api/teachers
    [HttpPost("")]
    public async Task<ActionResult<TeachersResponseDto>> AddTeacher(UpsertTeacherDto teacher)
    {
        var response = await service.AddTeacherAsync(teacher);

        if (response.Success) return Ok(response);

        return BadRequest(response);
    }

    //Update teacher by id
    //PUT /api/teachers/:id
    [HttpPut("{id:int}")]
    public async Task<ActionResult<TeachersResponseDto>> UpdateTeacher(int id, UpsertTeacherDto teacher)
    {
        var response = await service.UpdateTeacherAsync(id, teacher);

        if (response.Success) return Ok(response);

        if (response.Message == "Teacher not found") return NotFound(response);

        return BadRequest(response);
    }

    //Delete teacher by id
    //DELETE /api/teachers/:id
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<TeachersResponseDto>> DeleteTeacher(int id)
    {
        var response = await service.DeleteTeacherAsync(id);

        if (response.Success) return Ok(response);

        return NotFound(response);
    }
}