
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NPJL.Application.DTOs.Bookings;
using NPJL.Application.Services;
using NPJL.Infrastructure.Identity;

namespace NPJL.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController(
    BookingService bookingService,
    UserManager<ApplicationUser> userManager) : ControllerBase
{
    [HttpGet("my")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<List<BookingDto>>> GetMyBookings()
    {
        var userId = User.FindFirst(
            System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized();
        }

        var user = await userManager.FindByIdAsync(userId);

        if (user?.CustomerId == null)
        {
            return NotFound("Customer account not found.");
        }

        var bookings = await bookingService.GetByCustomerIdAsync(
            user.CustomerId.Value);

        return Ok(bookings);
    }
}
