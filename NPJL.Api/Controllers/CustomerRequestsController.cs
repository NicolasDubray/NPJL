using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NPJL.Application.Services;
using NPJL.Application.DTOs.Requests;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using NPJL.Infrastructure.Identity;
using NPJL.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace NPJL.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class CustomerRequestsController : ControllerBase
{
    private readonly CustomerRequestService _customerRequestService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    
    public CustomerRequestsController(
    CustomerRequestService customerRequestService,
    UserManager<ApplicationUser> userManager,
    ApplicationDbContext context)
    {
        _customerRequestService = customerRequestService;
        _userManager = userManager;
        _context = context;
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
    // CustomerId hämtas från den autentiserade användaren.
    // Denna endpoint används av kunder.
    [Authorize(Roles = "Customer")]
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateRequestDto request)
    {
        // Hämta Identity-användarens ID från JWT-token.
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            return Unauthorized();
        }

        // Hämta användaren från Identity.
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return Unauthorized();
        }

        // Kontrollera att användaren faktiskt är kopplad till en kund.
        if (user.CustomerId == null)
        {
            return Forbid();
        }

        // Sätt CustomerId server-side.
        // Vi litar inte på CustomerId som skickas från klienten.
        request.CustomerId = user.CustomerId.Value;
        // Kontrollera att bokningen tillhör den inloggade kunden.
        if (request.BookingId.HasValue)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.Id == request.BookingId.Value);

            if (booking == null || booking.CustomerId != user.CustomerId.Value)
            {
                return NotFound();
            }
        }
        // Kontrollera att fakturan tillhör den inloggade kunden.
        if (request.InvoiceId.HasValue)
        {
            var invoice = await _context.Invoices
                .FirstOrDefaultAsync(i => i.Id == request.InvoiceId.Value);

            if (invoice == null || invoice.CustomerId != user.CustomerId.Value)
            {
                return NotFound();
            }
        }
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