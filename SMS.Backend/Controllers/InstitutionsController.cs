using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Backend.Services.Institutions;
using SMS.Models.DTOs.Institutions;

namespace SMS.Backend.Controllers;

[ApiController]
[Route("/api/institutions")]
[Authorize]
public class InstitutionsController(IInstitutionService institutionService) : ControllerBase
{
    //Get all institutions
    //GET /api/institutions
    [HttpGet("")]
    public async Task<ActionResult<InstitutionsResponseDto>> GetInstitutions()
    {
        var response = await institutionService.GetInstitutionsAsync();

        return Ok(response);
    }

    //Get institution by id
    //GET /api/institutions/:id
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<InstitutionsResponseDto>> GetInstitution(int id)
    {
        var response = await institutionService.GetInstitutionByIdAsync(id);

        if (!response.Success)
            return NotFound(response);

        return Ok(response);
    }

    //Add new institution
    //POST /api/institutions
    [HttpPost("")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<InstitutionsResponseDto>> AddInstitution(UpsertInstitutionDto institutionDto)
    {
        var response = await institutionService.AddInstitutionAsync(institutionDto);

        if (response.Success) return CreatedAtRoute("", response);

        if (response.Message == "Institution with this name already exists")
            return Conflict(response);

        return BadRequest(response);
    }

    //Update institution by id
    //PUT /api/institutions/:id
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<InstitutionsResponseDto>> UpdateInstitution(int id,
        UpsertInstitutionDto institutionDto)
    {
        var response = await institutionService.UpdateInstitutionAsync(id, institutionDto);

        if (response.Success) return Ok(response);

        if (response.Message == "Institution not found")
            return NotFound(response);

        if (response.Message == "Institution with this name already exists")
            return Conflict(response);

        return BadRequest(response);
    }

    //Delete institution by id
    //DELETE /api/institutions/:id
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<InstitutionsResponseDto>> DeleteInstitution(int id)
    {
        var response = await institutionService.DeleteInstitutionAsync(id);

        if (!response.Success)
            return NotFound(response);

        return Ok(response);
    }
}