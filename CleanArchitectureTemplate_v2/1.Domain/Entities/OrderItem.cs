using _1.Domain.Commons;

namespace _1.Domain.Entities
{
    public class OrderItem : BaseAuditableEntity
    {
        public Guid ProductId { get; set; }
        public Guid OrderId { get; set; }
        public decimal Quantity { get; set; }
        public Product Product { get; set; }
        public Order Order { get; set; }
    }
}
