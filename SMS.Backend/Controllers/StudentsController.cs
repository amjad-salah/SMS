using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Backend.Services.Students;
using SMS.Backend.Utils;
using SMS.Models.DTOs.Students;

namespace SMS.Backend.Controllers;

[ApiController]
[Route("api/students")]
[Authorize]
public class StudentsController(
    IStudentsService service,
    AuthUtils authUtils) : ControllerBase
{
    //Get all students
    //GET /api/students
    [HttpGet("")]
    [Authorize(Roles = "Admin,Teacher,Registrar,Support")]
    public async Task<ActionResult<StudentsResponseDto>> GetStudents()
    {
        var response = await service.GetStudentsAsync();

        return Ok(response);
    }

    //Get student by id
    //GET /api/students/:id
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,Teacher,Registrar,Support,Student,Parent")]
    public async Task<ActionResult<StudentsResponseDto>> GetStudent(int id)
    {
        var toke = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        var userId = authUtils.GetUserIdFromToken(toke);

        var response = await service.GetStudentByIdAsync(id, userId);

        if (response.Success) return Ok(response);

        if (response.Message == "Student not found") return NotFound(response);

        return BadRequest(response);
    }

    //Get active users
    //GET /api/students/active
    [HttpGet("active")]
    [Authorize(Roles = "Admin,Teacher,Registrar,Support")]
    public async Task<ActionResult<StudentsResponseDto>> GetActiveStudents()
    {
        var response = await service.GetActiveStudentsAsync();

        return Ok(response);
    }

    //Add new student
    //POST /api/students
    [HttpPost("")]
    [Authorize(Roles = "Admin,Registrar")]
    public async Task<ActionResult<StudentsResponseDto>> AddStudent(UpsertStudentDto student)
    {
        var response = await service.AddStudentAsync(student);

        if (!response.Success) return BadRequest(response);

        return CreatedAtRoute("", response);
    }

    //Update student by id
    //PUT /api/students/:id
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,Registrar")]
    public async Task<ActionResult<StudentsResponseDto>> UpdateStudent(int id, UpsertStudentDto student)
    {
        var response = await service.UpdateStudentAsync(id, student);

        if (response.Success) return Ok(response);

        if (response.Message == "Student not found") return NotFound(response);

        return BadRequest(response);
    }

    //Delete student by id
    //DELETE /api/students/:id
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin,Registrar")]
    public async Task<ActionResult<StudentsResponseDto>> DeleteStudent(int id)
    {
        var response = await service.DeleteStudentAsync(id);

        if (response.Success) return Ok(response);

        return NotFound(response);
    }

    //Transfer one student by id
    //PUT /api/students/transfer
    [HttpPut("transfer/{id:int}")]
    [Authorize(Roles = "Admin,Registrar,Teacher")]
    public async Task<ActionResult<StudentsResponseDto>> TransferStudent(int id, TransferStudentDto transfer)
    {
        var toke = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        var userId = authUtils.GetUserIdFromToken(toke);

        var response = await service.TransferStudentAsync(transfer.StudentId, transfer.ClassId, userId);

        if (response.Success) return Ok(response);

        if (response.Message == "Student not found") return NotFound(response);

        return BadRequest(response);
    }

    //Transfer one student by id
    //PUT /api/students/transfer
    [HttpPut("transfer")]
    [Authorize(Roles = "Admin,Registrar,Teacher")]
    public async Task<ActionResult<StudentsResponseDto>> TransferStudents(TransferStudentsDto transfer)
    {
        var toke = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        var userId = authUtils.GetUserIdFromToken(toke);

        var response = await service.TransferStudentsAsync(transfer.StudentIds, transfer.ClassId, userId);

        if (response.Success) return Ok(response);

        if (response.Message == "Student not found") return NotFound(response);

        return BadRequest(response);
    }
}