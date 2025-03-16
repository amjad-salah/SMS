using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Backend.Services.ExamTypes;
using SMS.Models.DTOs.ExamTypes;

namespace SMS.Backend.Controllers;

[ApiController]
[Route("api/exam-types")]
[Authorize(Roles = "Admin,Support")]
public class ExamTypesController(IExamTypesService service) : ControllerBase
{
    //Get all types
    //GET /api/exam-types
    [HttpGet("")]
    public async Task<ActionResult<ExamTypesResponseDto>> GetExamTypes()
    {
        var response = await service.GetExamTypesAsync();

        return Ok(response);
    }

    //Get type by id
    //GET /api/exam-types/:id
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ExamTypesResponseDto>> GetExamResults(int id)
    {
        var response = await service.GetExamTypeByIdAsync(id);

        if (!response.Success)
            return NotFound(response);

        return Ok(response);
    }

    //Add new type
    //POST /api/exam-types
    [HttpPost("")]
    public async Task<ActionResult<ExamTypesResponseDto>> AddType(UpsertExamTypeDto examType)
    {
        var response = await service.AddExamTypeAsync(examType);

        if (!response.Success)
            return BadRequest(response);

        return CreatedAtRoute("", response);
    }

    //Delete type by id
    //DELETE /api/exam-types/:id
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ExamTypesResponseDto>> DeleteType(int id)
    {
        var response = await service.DeleteExamTypeAsync(id);

        if (!response.Success)
            return NotFound(response);

        return Ok(response);
    }
}