using NPJL.Application.DTOs.Bookings;
using System;
using System.Collections.Generic;
using System.Text;
using NPJL.Domain.Entities;

namespace NPJL.Application.Interfaces
{
    // Repository för hantering av bokningar.
    // Ärver gemensamma CRUD-operationer från IRepository.
    public interface IBookingRepository : IRepository<Booking>
    {
        // Här kan bokningsspecifika metoder läggas till.
    }
}

