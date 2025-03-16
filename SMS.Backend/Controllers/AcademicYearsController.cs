using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Backend.Services.AcademicYears;
using SMS.Models.DTOs.AcademicYears;

namespace SMS.Backend.Controllers;

[Route("api/academic-years")]
[ApiController]
[Authorize]
public class AcademicYearsController(IAcademicYearsService service) : ControllerBase
{
    //Get all years
    //GET /api/academic-years
    [HttpGet("")]
    public async Task<ActionResult<AcademicYearsResponseDto>> GetAcademicYears()
    {
        var response = await service.GetAcademicYearsAsync();

        return Ok(response);
    }

    //Get year by id
    //GET /api/academic-years/:id
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,Teacher,Registrar,Support")]
    public async Task<ActionResult<AcademicYearsResponseDto>> GetAcademicYear(int id)
    {
        var response = await service.GetAcademicYearByIdAsync(id);

        if (!response.Success)
            return NotFound(response);

        return Ok(response);
    }

    //Add new year
    //POST /api/academic-years
    [HttpPost("")]
    [Authorize(Roles = "Admin,Registrar")]
    public async Task<ActionResult<AcademicYearsResponseDto>> AddAcademicYear(UpsertAcademicYearDto year)
    {
        var response = await service.AddAcademicYearAsync(year);

        if (!response.Success)
            return BadRequest(response);

        return CreatedAtRoute("", response);
    }

    //Update year by id
    //PUT /api/academic-years/:id
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,Registrar")]
    public async Task<ActionResult<AcademicYearsResponseDto>> UpdateAcademicYear(int id, UpsertAcademicYearDto year)
    {
        var response = await service.UpdateAcademicYearAsync(id, year);

        if (response.Success) return Ok(response);

        if (response.Message == "Year not fount")
            return NotFound(response);

        return BadRequest(response);
    }

    //Delete year by id
    //DELETE /api/academic-years/:id
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin,Registrar")]
    public async Task<ActionResult<AcademicYearsResponseDto>> DeleteAcademicYear(int id)
    {
        var response = await service.DeleteAcademicYearAsync(id);

        if (response.Success) return Ok(response);

        return NotFound(response);
    }
}