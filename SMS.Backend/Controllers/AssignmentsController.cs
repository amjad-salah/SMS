using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Backend.Services.Assignments;
using SMS.Backend.Utils;
using SMS.Models.DTOs.Assignments;

namespace SMS.Backend.Controllers;

[ApiController]
[Route("api/assignments")]
[Authorize]
public class AssignmentsController(
    IAssignmentsService service,
    AuthUtils authUtils) : ControllerBase
{
    //Get all assignments
    //GET /api/assignments
    [HttpGet("")]
    public async Task<ActionResult<AssignmentResponseDto>> GetAssignments()
    {
        var response = await service.GetAssignmentsAsync();

        return Ok(response);
    }

    //Get assigment by id
    //GET /api/assignment/:id
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AssignmentResponseDto>> GetAssignment(int id)
    {
        var response = await service.GetAssignmentByIdAsync(id);

        if (!response.Success) return NotFound(response);

        return Ok(response);
    }

    //Add new assignment
    //POST /api/assignments
    [HttpPost("")]
    [Authorize(Roles = "Teacher")]
    public async Task<ActionResult<AssignmentResponseDto>> AddAssignment(UpsertAssignmentDto assignment)
    {
        var toke = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        var userId = authUtils.GetUserIdFromToken(toke);

        var response = await service.AddAssignmentAsync(assignment, userId);

        if (!response.Success) return BadRequest(response);

        return CreatedAtRoute("", response);
    }

    //Update assignment by id
    //PUT /api/assignments/:id
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Teacher")]
    public async Task<ActionResult<AssignmentResponseDto>> UpdateAssignment(int id, UpsertAssignmentDto assignment)
    {
        var response = await service.UpdateAssignmentAsync(id, assignment);

        if (response.Success) return Ok(response);

        if (response.Message == "Assignment not found") return NotFound(response);

        return BadRequest(response);
    }

    //Delete assignment by id
    //DELETE /api/assignments/:id
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Teacher")]
    public async Task<ActionResult<AssignmentResponseDto>> DeleteAssignment(int id)
    {
        var response = await service.DeleteAssignmentAsync(id);

        if (response.Success) return Ok(response);

        return NotFound(response);
    }
}