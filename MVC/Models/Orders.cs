namespace projekt.Models
{
    [Table(Name = "Orders")]
    public class Orders
    {
        [PrimaryKey(Skip = true)]
        public int id { get; set; }
        public string dateOfCreation { get; set; }
        public string state { get; set; }
        public double totalPrice { get; set; }
        [ForeignKey]
        public int userId { get; set; }
    }
}
