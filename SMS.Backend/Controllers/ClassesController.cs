using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Backend.Services.Attendances;
using SMS.Backend.Services.Classes;
using SMS.Models.DTOs.Attendances;
using SMS.Models.DTOs.Classes;

namespace SMS.Backend.Controllers;

[ApiController]
[Route("api/classes")]
[Authorize(Roles = "Admin,Teacher,Support")]
public class ClassesController(
    IClassesService service,
    IAttendancesService attendancesService) : ControllerBase
{
    //Get all classes
    //GET /api/classes
    [HttpGet("")]
    public async Task<ActionResult<ClassesResponseDto>> GetClasses()
    {
        var response = await service.GetClassesAsync();

        return Ok(response);
    }

    //Get class by id
    //GET /api/classes/:id
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClassesResponseDto>> GetClass(int id)
    {
        var response = await service.GetClassByIdAsync(id);

        if (!response.Success)
            return NotFound(response);

        return Ok(response);
    }

    //Add new class
    //POST /api/classes
    [HttpPost("")]
    public async Task<ActionResult<ClassesResponseDto>> AddClass(UpsertClassDto classDto)
    {
        var response = await service.AddClassAsync(classDto);

        if (!response.Success)
            return BadRequest(response);

        return CreatedAtRoute("", response);
    }

    //Update class by id
    //PUT /api/classes/:id
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ClassesResponseDto>> UpdateClass(int id, UpsertClassDto classDto)
    {
        var response = await service.UpdateClassAsync(id, classDto);

        if (response.Success) return Ok(response);

        if (response.Message == "Class not found")
            return NotFound(response);

        return BadRequest(response);
    }

    //Delete class by id
    //DELETE /api/classes/:id
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ClassesResponseDto>> DeleteClass(int id)
    {
        var response = await service.DeleteClassAsync(id);

        if (response.Success) return Ok(response);

        return NotFound(response);
    }

    //Add class attendances
    //POST /api/classes/:id/attendances
    [HttpPost("{id:int}/attendances")]
    public async Task<ActionResult<ClassesResponseDto>> AddClassAttendances(int id,
        AddClassAttendancesDto request)
    {
        var response = await attendancesService.AddClassAttendances(request);

        if (response.Success) return Ok(response.Adapt<ClassesResponseDto>());

        return BadRequest(response.Adapt<ClassesResponseDto>());
    }
}