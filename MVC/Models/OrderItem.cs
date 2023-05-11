namespace projekt.Models
{
    [Table(Name = "OrderItem")]
    public class OrderItem
    {
        [ForeignKey]
        public int orderId { get; set; }
        [ForeignKey]
        public int productId { get; set; }
        public int quantity { get; set; }
        public string name { get; set; }
    }
}
