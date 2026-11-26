
using System.Net.Http.Json;
using NPJL.Application.DTOs.Bookings;

namespace NPJL.Web.Services;

public class BookingApiService(AuthService authService)
{
    public async Task<List<BookingDto>> GetMyBookingsAsync()
    {
        var client = authService.CreateAuthenticatedClient();

        var bookings = await client.GetFromJsonAsync<List<BookingDto>>(
            "api/Bookings/my");

        return bookings ?? [];
    }
}
