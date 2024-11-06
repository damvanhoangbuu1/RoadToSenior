using _1.Domain.Commons;

namespace _1.Domain.Entities
{
    public class Product : BaseAuditableEntity
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; }
        public decimal ProductDiscount { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
