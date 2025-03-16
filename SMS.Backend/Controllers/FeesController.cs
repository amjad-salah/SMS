using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Backend.Services.Fees;
using SMS.Models.DTOs.Fees;

namespace SMS.Backend.Controllers;

[ApiController]
[Route("api/fees")]
[Authorize(Roles = "Admin,Accountant")]
public class FeesController(IFeesService service) : ControllerBase
{
    //Get all fess
    //GET /api/fees
    [HttpGet("")]
    public async Task<ActionResult<FeesResponseDto>> GetFees()
    {
        var response = await service.GetFeesAsync();

        return Ok(response);
    }

    //Get fee by id
    //GET /api/fess/:id
    [HttpGet("{id:int}")]
    public async Task<ActionResult<FeesResponseDto>> GetFee(int id)
    {
        var response = await service.GetFeesByIdAsync(id);

        if (!response.Success)
            return NotFound(response);

        return Ok(response);
    }

    //Add new fee
    //POST /api/fees
    [HttpPost("")]
    public async Task<ActionResult<FeesResponseDto>> AddFee(UpsertFeeDto fee)
    {
        var response = await service.AddFeeAsync(fee);

        if (!response.Success)
            return BadRequest(response);

        return CreatedAtRoute("", response);
    }

    //Delete fee by id
    //DELETE /api/fess/:id
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<FeesResponseDto>> DeleteFee(int id)
    {
        var response = await service.DeleteFeeAsync(id);

        if (!response.Success)
            return NotFound(response);

        return Ok(response);
    }
}