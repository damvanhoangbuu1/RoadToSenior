namespace _1.CleanArchitecture.Domain.Common
{
    public abstract class BaseAuditableEntity : IEntity
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}