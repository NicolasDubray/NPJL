
using NPJL.Application.Interfaces;

namespace NPJL.Application.Services
{
    // Gemensam service för grundläggande CRUD-operationer.
    public class GenericService<TEntity> where TEntity : class
    {
        protected readonly IRepository<TEntity> _repository;

        public GenericService(IRepository<TEntity> repository)
        {
            _repository = repository;
        }

        // Hämtar samtliga poster.
        public Task<IReadOnlyList<TEntity>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        // Hämtar en post via dess ID.
        public Task<TEntity?> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }
    }
}
