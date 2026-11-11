using Microsoft.EntityFrameworkCore;
using NPJL.Application.Interfaces;
using NPJL.Infrastructure.Persistence;

namespace NPJL.Infrastructure.Repositories
{
    public class Repository<TEntity>(ApplicationDbContext _context) : IRepository<TEntity>
        where TEntity : class
    {
        protected ApplicationDbContext Context => _context;

        public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync()
            => await _context.Set<TEntity>().ToListAsync();

        public virtual async Task<TEntity?> GetByIdAsync(int id)
            => await _context.Set<TEntity>().FindAsync(id);

        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);

            return entity;
        }

        public virtual Task UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);

            return Task.CompletedTask;
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);

            if (entity != null)
            {
                _context.Set<TEntity>().Remove(entity);
            }
        }
    }
}