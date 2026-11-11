using NPJL.Application.DTOs.Bookings;
using NPJL.Application.Interfaces;

namespace NPJL.Application.Services
{
    public class BookingService(IBookingRepository _bookingRepository)
    {
        public async Task<List<BookingDto>> GetByCustomerIdAsync(int customerId)
        {
            var bookings = await _bookingRepository.GetByCustomerIdAsync(customerId);
            return bookings.Select(b => new BookingDto
            {
                Id = b.Id,
                StartTime = b.StartTime,
                EndTime = b.EndTime,
                Status = b.Status.ToString()
            }).ToList();
        }

    }
}
