using mvcapp.Models;

namespace mvcapp.Services
{
    public class ProductsService
    {
        private List<Product> products = Product.GetProducts();

        public List<Product> List()
        {
            return products;
        }

        public Product? Get(int id)
        {
            return products.FirstOrDefault(p => p.Id == id);
        }
    }
}
