using GymManagementSystemDAL.Data.DbContexts;
using GymManagementSystemDAL.Data.Models;
using GymManagementSystemDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemDAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext _dbContext;
        private readonly Dictionary<String, object> repositories = [];
        public ISessionRepository SessionRepository { get; }

        public UnitOfWork(GymDbContext dbContext, ISessionRepository sessionRepository)
        {
            _dbContext = dbContext;
            SessionRepository = sessionRepository;
        }


        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var typeName = typeof(TEntity).Name; 
            if(repositories.TryGetValue(typeName,out var repository)){
                return (IGenericRepository<TEntity>)repository;
            }
            var repo = new GenericRepository<TEntity>(_dbContext);
            repositories[typeName] = repo;
            return repo;
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct)
        {
            return await _dbContext.SaveChangesAsync();  
        }
    }
}
