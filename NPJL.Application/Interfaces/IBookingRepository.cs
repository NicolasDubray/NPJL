using NPJL.Domain.Entities;

namespace NPJL.Application.Interfaces
{
    // Repository för hantering av bokningar.
    // Ärver gemensamma CRUD-operationer från IRepository.
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<IReadOnlyList<Booking>> GetByCustomerIdAsync(int customerId);
    }
}

