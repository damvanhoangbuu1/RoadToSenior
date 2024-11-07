using _1.Domain.Commons;
using _1.Domain.Interfaces;
using _3.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace _3.Infrastructure.Repositories
{
    public class UnitOfWork : IRepositoryFactory, IUnitOfWork
    {
        private bool disposed = false;
        /// <summary>
        /// The Transaction
        /// </summary>
        private IDbContextTransaction _objTran;
        /// <summary>
        /// The DbContext
        /// </summary>
        private readonly DbContext _dbContext;
        /// <summary>
        /// The Repository
        /// </summary>
        private Dictionary<Type, object> repositories = new Dictionary<Type, object>();
        /// <summary>
        /// Initializes a new instance of the UnitOfWork class
        /// </summary>
        public UnitOfWork(ApplicationDbContext dbContext) => _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity
        {
            var type = typeof(TEntity);
            if (!repositories.ContainsKey(type))
            {
                repositories[type] = new Repository<TEntity>(_dbContext);
            }

            return (IRepository<TEntity>)repositories[type];
        }

        public void CreateTransaction()
        {
            _objTran = _dbContext.Database.BeginTransaction();
        }
        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Commit()
        {
            _objTran?.Commit();
        }

        public async Task CommitAsync()
            => await _dbContext.SaveChangesAsync();


        public void Rollback()
        {
            _objTran?.Rollback();
            _dbContext.Dispose();
        }


        public async Task RollbackAsync()
        {
            await _dbContext.DisposeAsync();
        }

        public int ExecuteSqlRaw(string sql, params object[] parameters) => _dbContext.Database.ExecuteSqlRaw(sql, parameters);

        public IQueryable<TEntity> FromSqlRaw<TEntity>(string sql, params object[] parameters) where TEntity : class => _dbContext.Set<TEntity>().FromSqlRaw(sql, parameters);

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // clear repositories
                    if (repositories != null)
                    {
                        repositories.Clear();
                    }

                    // dispose the db context.
                    _objTran?.Dispose();
                    _dbContext.Dispose();
                }
            }

            disposed = true;
        }
    }
}
