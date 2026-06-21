using GymManagementSystemDAL.Data.DbContexts;
using GymManagementSystemDAL.Data.Models;
using GymManagementSystemDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemDAL.Repositories.Classes
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity, new()
    {
        private readonly GymDbContext _dbContext;
        private readonly DbSet<T> _dbSet; 
        public GenericRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set <T>();
        }
        public void AddAsync(T entity)
        {
            _dbSet.Add(entity);

        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct)
        {
             return await _dbSet.AnyAsync(predicate, ct);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>>? preducate = null , CancellationToken ct = default)
        {
            return  preducate is null ?await _dbSet.AsNoTracking().CountAsync(ct) :await _dbSet.AsNoTracking().CountAsync(ct);
        }

        public void DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool tracking = false, CancellationToken ct = default)
        {
            var result = tracking ? _dbSet.FirstOrDefaultAsync(predicate, ct) : _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate, ct);
            return await result;
        }

        public async Task<IEnumerable<T>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            var entities = tracking ? await _dbSet.ToListAsync(ct) : await _dbSet.AsNoTracking().ToListAsync(ct);
            return entities;
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, bool tracking = false, CancellationToken ct = default)
        {

            if (predicate is null)
            {
                var entities = tracking ? await _dbSet.ToListAsync(ct) : await _dbSet.AsNoTracking().ToListAsync(ct);
                return entities;
            }
            else
            {
                var entities = tracking ? await _dbSet.ToListAsync(ct) : await _dbSet.AsNoTracking().ToListAsync(ct);
                return entities;
            }
        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _dbSet.FindAsync(id, ct);
            return entity;
        }

        public void UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
