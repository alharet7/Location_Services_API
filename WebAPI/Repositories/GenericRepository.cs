using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using WebAPI.Entities;
using WebAPI.Entities.Enums;
using WebAPI.Interfaces;

namespace WebAPI.Repositories
{
    public class GenericRepository<T> : IGenericRepository where T : ApplicationDbContext
    {
        private readonly T _dbContext;

        public GenericRepository(T dbContext)
        {
            _dbContext = dbContext;
        }

        public TEntity Add<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            entity.UpdateStatus = UpdateStatus.New;
            _dbContext.Add<TEntity>(entity);
            _dbContext.SaveChanges();
            return entity;
        }

        public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class, IEntity
        {
            return _dbContext.Set<TEntity>().Where(e => e.UpdateStatus != UpdateStatus.Deleted);
        }

        public TEntity GetById<TEntity>(int id) where TEntity : class, IEntity
        {
            return _dbContext.Set<TEntity>()
                     .FirstOrDefault(e => e.Id == id && e.UpdateStatus != UpdateStatus.Deleted);
        }



        public void Update<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            entity.UpdateStatus = UpdateStatus.Updated;
            _dbContext.Set<TEntity>().Update(entity);
            _dbContext.SaveChanges();
        }

        public void DeleteEntity<TEntity>(int id) where TEntity : class, IEntity
        {
            var entity = _dbContext.Set<TEntity>().FirstOrDefault(e => e.Id == id);
            if (entity != null)
            {
                entity.UpdateStatus = UpdateStatus.Deleted;
                _dbContext.Set<TEntity>().Update(entity);
                _dbContext.SaveChanges();
            }
        }


        // ----------------------------- *** Transaction Methods *** -------------------------------

        public async Task BeginTransactionAsync()
        {
            await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
           await _dbContext.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _dbContext.Database.RollbackTransactionAsync();
        }
    }
}
