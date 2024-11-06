using _1.Domain.Commons;
using System.Linq.Expressions;

namespace _1.Domain.Interfaces.Commons
{
    public interface IRepository<T> where T : class, IEntity
    {
        IQueryable<T> GetIQueryable(bool isIncludeDeleted = false);

        T Get(Expression<Func<T, bool>> expression);

        Task<T> GetAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);

        IEnumerable<T> GetAll();

        IEnumerable<T> GetAll(Expression<Func<T, bool>> expression);

        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);

        void Add(T entity);

        void AddRange(IEnumerable<T> entities);

        Task AddAsync(T entity, CancellationToken cancellationToken = default);

        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);

        void Update(T entity);

        void UpdateRange(IEnumerable<T> entities);

        T Insert(T entity);

        Task<T> InsertAsync(T entity);

        void SaveChanges();

        Task SaveChangesAsync();

        //Task<PaginatedList<T>> GetRows(int page = 1, int postsPerPage = 10);
    }
}