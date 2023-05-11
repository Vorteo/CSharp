using mvcapp.Models;

namespace mvcapp.Services
{
    public class ShoppingCartService
    {
        private List<Product> products = new List<Product>();

        public void Add(Product product)
        {
            products.Add(product);
        }

        public List<Product> List() 
        {
            return products;
        }
        public void Clear()
        {
            products.Clear();
        }
        public int Count { get { return products.Count; } }
    }
}
