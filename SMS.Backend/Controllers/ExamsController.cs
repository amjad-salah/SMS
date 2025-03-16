using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Backend.Services.Exams;
using SMS.Models.DTOs.Exams;

namespace SMS.Backend.Controllers;

[ApiController]
[Route("/api/exams")]
[Authorize(Roles = "Admin,Teacher,Support")]
public class ExamsController(IExamsService service) : ControllerBase
{
    //Get all exams
    //GET /api/exams
    [HttpGet("")]
    public async Task<ActionResult<ExamsResponseDto>> GetExams()
    {
        var response = await service.GetExamsAsync();

        return Ok(response);
    }

    //Get exams by active years
    //GET /api/exams/active-years
    [HttpGet("active-years")]
    public async Task<ActionResult<ExamsResponseDto>> GetExamsCurrentYear()
    {
        var response = await service.GetExamsByCurrentYearAsync();

        return Ok(response);
    }

    //Get exam by id
    //GET /api/exams/:id
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ExamsResponseDto>> GetExamById(int id)
    {
        var response = await service.GetExamByIdAsync(id);

        if (!response.Success)
            return NotFound(response);

        return Ok(response);
    }

    //Add new exam
    //POST /api/exams
    [HttpPost("")]
    public async Task<ActionResult<ExamsResponseDto>> AddExam(UpsertExamDto exam)
    {
        var response = await service.AddExamAsync(exam);

        if (!response.Success)
            return BadRequest(response);

        return CreatedAtRoute("", response);
    }

    //Update exam by id
    //PUT /api/exams/:id
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ExamsResponseDto>> UpdateExam(int id, UpsertExamDto exam)
    {
        var response = await service.UpdateExamAsync(id, exam);

        if (response.Success) return Ok(response);

        if (response.Message == "Exam not found")
            return NotFound(response);

        return BadRequest(response);
    }

    //Delete exam by id
    //DELETE /api/exams/:id
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ExamsResponseDto>> DeleteExam(int id)
    {
        var response = await service.DeleteExamAsync(id);

        if (!response.Success)
            return NotFound(response);

        return Ok(response);
    }
}