using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Backend.Services.Grades;
using SMS.Models.DTOs.Grades;

namespace SMS.Backend.Controllers;

[ApiController]
[Route("/api/grades")]
[Authorize(Roles = "Admin,Support")]
public class GradesController(IGradesService service) : ControllerBase
{
    //Get all grades
    //GET /api/grades
    [HttpGet("")]
    public async Task<ActionResult<GradesResponseDto>> GetGrades()
    {
        var response = await service.GetGradesAsync();

        return Ok(response);
    }

    //Get grade by id
    //GET /api/grades/:id
    [HttpGet("{id:int}")]
    public async Task<ActionResult<GradesResponseDto>> GetGrade(int id)
    {
        var response = await service.GetGradeByIdAsync(id);

        if (!response.Success)
            return NotFound(response);

        return Ok(response);
    }

    //Add new grade
    //POST /api/grades
    [HttpPost("")]
    public async Task<ActionResult<GradesResponseDto>> AddGrade(UpsertGradeDto grade)
    {
        var response = await service.AddGradeAsync(grade);

        if (!response.Success)
            return BadRequest(response);

        return CreatedAtRoute("", response);
    }

    //Update grade by id
    //PUT /api/grades/:id
    [HttpPut("{id:int}")]
    public async Task<ActionResult<GradesResponseDto>> UpdateGrade(int id, UpsertGradeDto grade)
    {
        var response = await service.UpdateGradeAsync(id, grade);

        if (response.Success) return Ok(response);

        if (response.Message == "Grade not found")
            return NotFound(response);

        return BadRequest(response);
    }

    //Delete grade by id
    //DELETE /api/grades/:id
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<GradesResponseDto>> DeleteGrade(int id)
    {
        var response = await service.DeleteGradeAsync(id);

        if (!response.Success)
            return NotFound(response);

        return Ok(response);
    }
}