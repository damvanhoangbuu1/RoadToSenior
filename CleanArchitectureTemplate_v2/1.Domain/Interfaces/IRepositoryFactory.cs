using _1.Domain.Commons;

namespace _1.Domain.Interfaces
{
    public interface IRepositoryFactory
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity;
    }
}
