using Microsoft.AspNetCore.Mvc;
using mvcapp.Models;
using mvcapp.Services;
using System.Xml;
using System.Xml.Serialization;

namespace mvcapp.Controllers
{
    public class ShoppingCartController : Controller
    {
        private ShoppingCartService cart;
        public ShoppingCartController(ShoppingCartService cart)
        {
            this.cart = cart;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(ShoppingCartForm form)
        {
            if(ModelState.IsValid)
            {
                HttpContext.Session.Clear();
                cart.Clear();
                return RedirectToAction("Done");
            }
            return View();
        }

        public IActionResult Done()
        {
            return View();
        }
        public IActionResult GetProducts([FromServices] ShoppingCartService cart)
        {
            return new JsonResult(cart.List());
        }
        public IActionResult GetXml()
        {

            MemoryStream stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(typeof(List<Product>));
            xml.Serialize(stream, cart.List());

            stream.Seek(0, SeekOrigin.Begin);



            return new FileStreamResult(stream, "text/xml");
        }
    }
}
