namespace projekt.Models
{
    [Table(Name = "ProductCategory")]
    public class ProductCategory
    {
        [ForeignKey]
        public int productId { get; set; }
        [ForeignKey]
        public int categoryId { get; set; }
    }
}
