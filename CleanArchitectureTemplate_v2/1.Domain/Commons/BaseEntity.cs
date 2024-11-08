namespace _1.Domain.Commons
{
    public abstract class BaseEntity : IEntity
    {
        public Guid Id { get; set; }
    }
}