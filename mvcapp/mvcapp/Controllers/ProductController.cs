using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using mvcapp.Services;
using System.Text;
using System.Text.Json;

namespace mvcapp.Controllers
{
    public class ProductController : Controller
    {
        private ProductsService productsService;
        private ShoppingCartService cart;
        public ProductController(ProductsService productsService, ShoppingCartService cart)
        {
            this.productsService = productsService;
            this.cart = cart;
        }

        public IActionResult Index()
        {
            var products = productsService.List();
            ViewBag.Products = products;
            return View();
        }

        public IActionResult Detail(int id)
        {
            var product = productsService.Get(id);
            if(product == null)
            {
                return NotFound();
            }
            ViewBag.Product = product;
            return View();
        }
        public IActionResult AddToCart(int id)
        {
            List<int> productsIds = new List<int>();

            if (HttpContext.Session.TryGetValue("ProductsIds", out byte[] data))
            {
                productsIds = JsonSerializer.Deserialize<List<int>>(Encoding.UTF8.GetString(data));
            }

            productsIds.Add(id);
            HttpContext.Session.Set("ProductsIds", Encoding.UTF8.GetBytes(JsonSerializer.Serialize(productsIds)));

            var product = productsService.Get(id);
            cart.Add(product);
            return RedirectToAction("Index");

        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewBag.ProductCount = cart.Count;
        }
    }
}
