using _1.Domain.Commons;

namespace _1.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
        Task SaveAsync();
        void Commit();
        void Rollback();
        Task CommitAsync();
        Task RollbackAsync();
        void CreateTransaction();
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity;
        public int ExecuteSqlRaw(string sql, params object[] parameters);
        public IQueryable<TEntity> FromSqlRaw<TEntity>(string sql, params object[] parameters) where TEntity : class;
    }
}
