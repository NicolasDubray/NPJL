using System;
using System.Collections.Generic;
using System.Text;

namespace NPJL.Infrastructure.Repositories
{
    public class BookingRepository
    {
        public Task<IReadOnlyList<BookingRepository>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
        public Task<BookingRepository?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<BookingRepository> CreateAsync(BookingRepository booking)
        {
            throw new NotImplementedException();
        }
        public Task UpdateAsync(BookingRepository booking)
        {
            throw new NotImplementedException();
        }
        public Task removeAsync(int id)
        {
            throw new NotImplementedException();
        }

    }
}
