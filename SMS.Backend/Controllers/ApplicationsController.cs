using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Backend.Services.Applications;
using SMS.Models.DTOs.Applications;

namespace SMS.Backend.Controllers;

[ApiController]
[Route("api/applications")]
[Authorize(Roles = "Admin,Registrar")]
public class ApplicationsController(IApplicationsService service) : ControllerBase
{
    //Get all applications
    //GET /api/applications
    [HttpGet("")]
    public async Task<ActionResult<ApplicationsResponseDto>> GetApplications()
    {
        var response = await service.GetApplicationsAsync();

        return Ok(response);
    }

    //Get all applications by current year
    //GET /api/applications/current-year
    [HttpGet("current-year")]
    public async Task<ActionResult<ApplicationsResponseDto>> GetApplicationsByCurrentYear()
    {
        var response = await service.GetApplicationsByCurrentYearAsync();

        return Ok(response);
    }

    //Get application by id
    //GET /api/applications/:id
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApplicationsResponseDto>> GetApplication(int id)
    {
        var response = await service.GetApplicationByIdAsync(id);

        if (response.Success) return Ok(response);

        return NotFound(response);
    }

    //Add new application
    //POST /api/applications
    [HttpPost("")]
    public async Task<ActionResult<ApplicationsResponseDto>> AddApplication(UpsertApplicationDto application)
    {
        var response = await service.AddApplicationAsync(application);

        if (response.Success) return Ok(response);

        return BadRequest(response);
    }

    //Update application by id
    //PUT /api/applications
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApplicationsResponseDto>> UpdateApplication(int id,
        UpsertApplicationDto application)
    {
        var response = await service.UpdateApplicationAsync(id, application);

        if (response.Success) return Ok(response);

        if (response.Message == "Application not found")
            return NotFound(response);

        return BadRequest(response);
    }

    //Delete application by id
    //DELETE /api/applications/:id
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApplicationsResponseDto>> DeleteApplication(int id)
    {
        var response = await service.DeleteApplicationAsync(id);

        if (response.Success) return Ok(response);

        return NotFound(response);
    }

    //Approve application by id
    //GET /api/applications/:id/approve
    [HttpGet("{id:int}/approve")]
    public async Task<ActionResult<ApplicationApproverResponseDto>> ApplicationApprove(int id,
        ApplicationApproveDto approveDto)
    {
        var response = await service.ApproveApplicationsAsync(id, approveDto);

        if (response.Success) return Ok(response);

        return NotFound(response);
    }
}