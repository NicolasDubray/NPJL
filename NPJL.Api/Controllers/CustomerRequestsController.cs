using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NPJL.Application.Services;
using NPJL.Application.DTOs.Requests;

namespace NPJL.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrator")]
public class CustomerRequestsController : ControllerBase
{
    private readonly CustomerRequestService _customerRequestService;

    public CustomerRequestsController(
        CustomerRequestService customerRequestService)
    {
        _customerRequestService = customerRequestService;
    }

    // Hämtar samtliga kundförfrågningar.
    // Endast administratörer har åtkomst.
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var requests = await _customerRequestService.GetAllAsync();

        return Ok(requests);
    }
    // Ändrar status på en kundförfrågan.
    // Endast administratörer har åtkomst.
    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        [FromBody] UpdateRequestStatusDto request)
    {
        try
        {
            await _customerRequestService.UpdateStatusAsync(
                id,
                request.Status);

            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentException)
        {
            return BadRequest();
        }
    }
}