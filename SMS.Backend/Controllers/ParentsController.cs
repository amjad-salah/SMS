using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Backend.Services.Parents;
using SMS.Models.DTOs.Parents;

namespace SMS.Backend.Controllers;

[ApiController]
[Route("api/parents")]
[Authorize(Roles = "Admin,Support,Registrar")]
public class ParentsController(IParentsService service) : ControllerBase
{
    //Get all parents
    //GET /api/parents
    [HttpGet("")]
    public async Task<ActionResult<ParentsResponseDto>> GetAllParents()
    {
        var response = await service.GetParentsAsync();

        return Ok(response);
    }

    //Get parent by id
    //GET /api/parents/:id
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ParentsResponseDto>> GetParent(int id)
    {
        var response = await service.GetParentByIdAsync(id);

        if (!response.Success)
            return NotFound(response);

        return Ok(response);
    }

    //Add new parent
    //POST /api/parents
    [HttpPost("")]
    public async Task<ActionResult<ParentsResponseDto>> AddParent(UpsertParentDto parent)
    {
        var response = await service.AddParentAsync(parent);

        if (!response.Success)
            return BadRequest(response);

        return CreatedAtRoute("", response);
    }

    //Update parents by id
    //PUT /api/parents/:id
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ParentsResponseDto>> UpdateParent(int id, UpsertParentDto parent)
    {
        var response = await service.UpdateParentAsync(id, parent);

        if (response.Success) return Ok(response);

        if (response.Message == "Parent not found")
            return NotFound(response);

        return BadRequest(response);
    }

    //Delete parent by id
    //DELETE /api/parents/:id
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ParentsResponseDto>> DeleteParent(int id)
    {
        var response = await service.DeleteParentAsync(id);

        if (!response.Success) return NotFound(response);

        return Ok(response);
    }
}