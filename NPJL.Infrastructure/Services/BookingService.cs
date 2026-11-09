using System;
using System.Collections.Generic;
using System.Text;
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;

namespace NPJL.Infrastructure.Services
{
    // Service för hantering av bokningar.
    // Ärver grundläggande hämtningsmetoder från GenericService.
    public class BookingService : GenericService<Booking>
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService(
            IBookingRepository repository)
            : base(repository)
        {
            _bookingRepository = repository;
        }
    }
}