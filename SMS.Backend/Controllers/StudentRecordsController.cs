using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Backend.Services.StudentRecords;
using SMS.Backend.Utils;
using SMS.Models.DTOs.StudentRecords;

namespace SMS.Backend.Controllers;

[ApiController]
[Route("api/student-records")]
[Authorize]
public class StudentRecordsController(
    IStudentRecordsService service,
    AuthUtils authUtils) : ControllerBase
{
    //Get all records
    //GET /api/students-records
    [HttpGet("")]
    [Authorize(Roles = "Admin,Teacher,Support")]
    public async Task<ActionResult<StudentRecordsResponseDto>> GetStudentsRecords()
    {
        var response = await service.GetRecordsAsync();

        return Ok(response);
    }

    //Get all student's records
    //GET /api/students-records/:studentId
    [HttpGet("{studentId:int}")]
    [Authorize(Roles = "Admin,Teacher,Support,Parent,Student")]
    public async Task<ActionResult<StudentRecordsResponseDto>> GetStudentRecords(int studentId)
    {
        var toke = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        var userId = authUtils.GetUserIdFromToken(toke);

        var response = await service.GetRecordsByStudentAsync(studentId, userId);

        if (response.Success) return Ok(response);

        return BadRequest(response);
    }

    //Get active years records
    //GET /api/students-records/active-years
    [HttpGet("active-years")]
    [Authorize(Roles = "Admin,Teacher,Support")]
    public async Task<ActionResult<StudentRecordsResponseDto>> GetStudentRecordsByCurrentYear()
    {
        var response = await service.GetRecordsByCurrentYearAsync();

        return Ok(response);
    }

    //Add Students Record
    //POST /api/students-records
    [HttpPost("")]
    [Authorize(Roles = "Admin,Teacher,Support")]
    public async Task<ActionResult<StudentRecordsResponseDto>> CreateStudentsRecords(
        AddStudentsRecordsDto addStudentsRecordsDto)
    {
        var response = await service.AddStudentsRecordsAsync(addStudentsRecordsDto.StudentIds,
            addStudentsRecordsDto.ExamTypeId);

        if (!response.Success) return BadRequest(response);

        return Ok(response);
    }
}