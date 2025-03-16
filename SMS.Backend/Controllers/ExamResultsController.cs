using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Backend.Services.ExamResults;
using SMS.Models.DTOs.ExamResults;

namespace SMS.Backend.Controllers;

[ApiController]
[Route("api/exam-results")]
[Authorize]
public class ExamResultsController(IExamResultsService service) : ControllerBase
{
    //Get all results
    //GET /api/exam-results
    [HttpGet("")]
    [Authorize(Roles = "Admin, Teacher")]
    public async Task<ActionResult<ExamResultsResponseDto>> GetResults()
    {
        var response = await service.GetResultsAsync();

        return Ok(response);
    }

    //Get result by id
    //GET /api/exam-results/:id
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin, Teacher")]
    public async Task<ActionResult<ExamResultsResponseDto>> GetResult(int id)
    {
        var response = await service.GetResultByIdAsync(id);

        if (!response.Success)
            return NotFound(response);

        return Ok(response);
    }

    //Get results by active years
    //GET /api/exam-results/active-years
    [HttpGet("active-years")]
    [Authorize(Roles = "Admin, Teacher")]
    public async Task<ActionResult<ExamResultsResponseDto>> GetResultsBtCurrentYear()
    {
        var response = await service.GetResultByCurrentYearAsync();

        return Ok(response);
    }

    //Add new result
    //POST /api/exam-results
    [HttpPost("")]
    [Authorize(Roles = "Admin, Teacher")]
    public async Task<ActionResult<ExamResultsResponseDto>> AddResult(UpsertExamResultDto result)
    {
        var response = await service.AddResultAsync(result);

        if (!response.Success)
            return BadRequest(response);

        return CreatedAtRoute("", response);
    }

    //Update result by id
    //PUT /api/exam-results/:id
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin, Teacher")]
    public async Task<ActionResult<ExamResultsResponseDto>> UpdateResult(int id, UpsertExamResultDto result)
    {
        var response = await service.UpdateResultAsync(id, result);

        if (response.Success) return Ok(response);

        if (response.Message == "Exam result not found")
            return NotFound(response);

        return BadRequest(response);
    }

    //Delete result by id
    //DELETE /api/exam-results/:id
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin, Teacher")]
    public async Task<ActionResult<ExamResultsResponseDto>> DeleteResult(int id)
    {
        var response = await service.DeleteResultAsync(id);

        if (response.Success) return Ok(response);

        return NotFound(response);
    }
}