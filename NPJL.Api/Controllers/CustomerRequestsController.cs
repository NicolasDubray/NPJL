using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NPJL.Application.Services;
using NPJL.Application.DTOs.Requests;

namespace NPJL.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

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
    [Authorize(Roles = "Administrator")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var requests = await _customerRequestService.GetAllAsync();
        
        return Ok(requests);
    }
    // Skapar en ny kundförfrågan.
    // Denna endpoint används av kunder.
    [Authorize(Roles = "Customer")]
    [HttpPost]
    public async Task<IActionResult> Create(
    [FromBody] CreateRequestDto request)
    {
        try
        {
            await _customerRequestService.CreateAsync(request);

            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    // Ändrar status på en kundförfrågan.
    // Endast administratörer har åtkomst.
    [Authorize(Roles = "Administrator")]
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