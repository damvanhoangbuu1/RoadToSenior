using _1.Domain.Commons;
using _1.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace _3.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        protected readonly DbContext _dbContext;
        private readonly DbSet<T> _entitiySet;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _entitiySet = _dbContext.Set<T>();
        }

        public void Add(T entity)
        {
            Repository<T>.SetId(entity);
            _dbContext.Add(entity);
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            Repository<T>.SetId(entity);
            await _dbContext.AddAsync(entity, cancellationToken);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Repository<T>.SetId(entity);
            }

            _dbContext.AddRange(entities);
        }

        public Task<T> InsertAsync(T entity)
        {
            return Task.FromResult(Insert(entity));
        }

        public T Insert(T entity)
        {
            Repository<T>.SetId(entity);
            _dbContext.Entry(entity).State = EntityState.Added;

            return _entitiySet.Add(entity).Entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                Repository<T>.SetId(entity);
            }

            await _dbContext.AddRangeAsync(entities, cancellationToken);
        }

        public T Get(Expression<Func<T, bool>> expression)
        {
            return _entitiySet.FirstOrDefault(expression);
        }

        public IEnumerable<T> GetAll()
            => _entitiySet.AsEnumerable();

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> expression)
            => _entitiySet.Where(expression).AsEnumerable();

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _entitiySet.ToListAsync(cancellationToken);

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
            => await _entitiySet.Where(expression).ToListAsync(cancellationToken);

        public async Task<T> GetAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await _entitiySet.FirstOrDefaultAsync(expression, cancellationToken);
        }

        public void Remove(T entity)
            => _dbContext.Remove(entity);

        public void RemoveRange(IEnumerable<T> entities)
            => _dbContext.RemoveRange(entities);

        public void Update(T entity)
        {
            _dbContext.Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _dbContext.UpdateRange(entities);
        }

        public IQueryable<T> GetIQueryable(bool isIncludeDeleted = false)
        {
            return !isIncludeDeleted ? _entitiySet : _dbContext.Set<T>().IgnoreQueryFilters();
        }

        private static void SetId(T entity)
        {
            entity.Id = entity?.Id != null && entity?.Id != Guid.Empty ? entity.Id : Guid.NewGuid();
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        //public async Task<PaginatedList<T>> GetRows(
        //    int page = 1, int postsPerPage = 10)
        //{
        //    return await PaginatedList<T>.CreateAsync(
        //        _entitiySet.OrderBy(x => x.Id), page, postsPerPage);
        //}
    }
}