using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using WebAPI.Interfaces;

namespace WebAPI.Repositories
{
    public interface IGenericRepository
    {
        public IQueryable<TEntity> GetAll<TEntity>()where TEntity : class, IEntity;

        public TEntity GetById<TEntity>(int id) where TEntity : class, IEntity;

        public TEntity Add<TEntity>(TEntity entity) where TEntity : class, IEntity;

        public void Update<TEntity>(TEntity entity) where TEntity : class,IEntity;
        public void DeleteEntity<TEntity>(int id) where TEntity : class, IEntity;

        public Task BeginTransactionAsync();
        public Task CommitTransactionAsync();
        public Task RollbackTransactionAsync();
    }
}
