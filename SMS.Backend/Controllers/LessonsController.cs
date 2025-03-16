using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Backend.Services.Lessons;
using SMS.Models.DTOs.Lessons;

namespace SMS.Backend.Controllers;

[ApiController]
[Route("api/lessons")]
[Authorize(Roles = "Admin,Teacher,Support")]
public class LessonsController(ILessonsService service) : ControllerBase
{
    //Get all lessons
    //GET /api/lessons
    [HttpGet("")]
    public async Task<ActionResult<LessonsResponseDto>> GetLessons()
    {
        var response = await service.GetLessonsAsync();

        return Ok(response);
    }

    //Get lesson by id
    //GET /api/lessons/:id
    [HttpGet("{id:int}")]
    public async Task<ActionResult<LessonsResponseDto>> GetLesson(int id)
    {
        var response = await service.GetLessonByIdAsync(id);

        if (!response.Success)
            return NotFound(response);

        return Ok(response);
    }

    //Add new lesson
    //POST /api/lessons
    [HttpPost("")]
    public async Task<ActionResult<LessonsResponseDto>> AddLesson(UpsertLessonDto lesson)
    {
        var response = await service.AddLessonAsync(lesson);

        if (!response.Success)
            return BadRequest(response);

        return CreatedAtRoute("", response);
    }

    //Update lesson by id
    //PUT /api/lessons/:id
    [HttpPut("{id:int}")]
    public async Task<ActionResult<LessonsResponseDto>> UpdateLesson(int id, UpsertLessonDto lesson)
    {
        var response = await service.UpdateLessonAsync(id, lesson);

        if (response.Success) return Ok(response);

        if (response.Message == "Lesson not found")
            return NotFound(response);

        return BadRequest(response);
    }

    //Delete lesson by id
    //DELETE /api/lessons/:id
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<LessonsResponseDto>> DeleteLesson(int id)
    {
        var response = await service.DeleteLessonAsync(id);

        if (!response.Success) return NotFound(response);

        return Ok(response);
    }
}