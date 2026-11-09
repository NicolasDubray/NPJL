using System;
using System.Collections.Generic;
using System.Text;

namespace NPJL.Application.Interfaces
{
    // Gemensamt kontrakt för grundläggande CRUD-operationer.
    // TEntity anger vilken typ av entitet repositoryt hanterar.
    public interface IRepository<TEntity> where TEntity : class
    {
        // Hämtar samtliga poster.
        Task<IReadOnlyList<TEntity>> GetAllAsync();

        // Hämtar en post via dess ID.
        Task<TEntity?> GetByIdAsync(int id);

        // Skapar en ny post.
        Task<TEntity> CreateAsync(TEntity entity);

        // Uppdaterar en befintlig post.
        Task UpdateAsync(TEntity entity);

        // Tar bort en post via dess ID.
        Task DeleteAsync(int id);
    }
}
