using _1.Domain.Commons;
using _1.Domain.Enums;

namespace _1.Domain.Entities
{
    public class Order : BaseAuditableEntity
    {
        public Guid UserId { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalMoney {  get; set; }
        public List<OrderItem> Items { get; set;} = new List<OrderItem>();
    }
}
