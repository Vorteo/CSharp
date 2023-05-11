namespace projekt.Models
{
    [Table(Name = "Category")]
    public class Category
    {
        [PrimaryKey(Skip = true)]
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}
