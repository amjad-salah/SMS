using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Backend.Services.Subjects;
using SMS.Models.DTOs.Subjects;

namespace SMS.Backend.Controllers;

[ApiController]
[Route("api/subjects")]
[Authorize(Roles = "Admin,Support")]
public class SubjectsController(ISubjectsService service) : ControllerBase
{
    //Get all subjects
    //GET /api/subjects
    [HttpGet("")]
    public async Task<ActionResult<SubjectsResponseDto>> GetSubjects()
    {
        var response = await service.GetSubjectsAsync();

        return Ok(response);
    }

    //Get subject by id
    //GET /api/subjects/:id
    [HttpGet("{id:int}")]
    public async Task<ActionResult<SubjectsResponseDto>> GetSubject(int id)
    {
        var response = await service.GetSubjectByIdAsync(id);

        if (!response.Success) return NotFound(response);

        return Ok(response);
    }

    //Add new subject
    //POST /api/subjects
    [HttpPost("")]
    public async Task<ActionResult<SubjectsResponseDto>> AddSubject(UpsertSubjectDto subject)
    {
        var response = await service.AddSubjectAsync(subject);

        if (!response.Success) return BadRequest(response);

        return CreatedAtRoute("", response);
    }

    //Update subject by id
    //PUT /api/subjects/:id
    [HttpPut("{id:int}")]
    public async Task<ActionResult<SubjectsResponseDto>> UpdateSubject(int id, UpsertSubjectDto subject)
    {
        var response = await service.UpdateSubjectAsync(id, subject);

        if (response.Success) return Ok(response);

        if (response.Message == "Subject not found!") return NotFound(response);

        return BadRequest(response);
    }

    //Delete subject by id
    //DELETE /api/subjects/:id
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<SubjectsResponseDto>> DeleteSubject(int id)
    {
        var response = await service.DeleteSubjectAsync(id);

        if (response.Success) return Ok(response);

        return NotFound(response);
    }
}