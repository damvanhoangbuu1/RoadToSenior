using _1.CleanArchitecture.Domain.Common;
using _2.CleanArchitecture.Application.DTOs.Common;
using System.Linq.Expressions;

namespace _2.CleanArchitecture.Application.Common.Interfaces
{
    public interface IGenericRepository<T> where T : class, IEntity
    {
        IQueryable<T> GetIQueryable(bool isIncludeDeleted = false);

        T Get(Expression<Func<T, bool>> expression);

        Task<T> GetAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);

        IEnumerable<T> GetAll();

        IEnumerable<T> GetAll(Expression<Func<T, bool>> expression);

        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);

        Task<PaginatedList<T>> GetRows(int page = 1, int postsPerPage = 10);

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
    }
}