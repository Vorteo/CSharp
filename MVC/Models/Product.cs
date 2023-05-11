using projekt.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace projekt.Models
{
    [Table(Name = "Product")]
    public class Product
    {
        [PrimaryKey(Skip = true)]
        public int id { get; set; }

        [Display(Name = "Název")]
        [Required(ErrorMessage = "Název musí být zadán")]
        public string name { get; set; }

        [Display(Name = "Výrobce")]
        [Required(ErrorMessage = "Výrobce musí být zadán")]
        public string manufacturer { get; set; }

        [Display(Name = "Popis")]
        [Required(ErrorMessage = "Popis musí být zadán")]
        public string description { get; set; }

        [Display(Name = "Barva")]
        [Required(ErrorMessage = "Barva musí být zadána")]
        public string colour { get; set; }

        [Display(Name = "Váha")]
        [Required(ErrorMessage = "Váha musí být zadána")]
        public int weight { get; set; }

        [Display(Name = "Cena")]
        [Required(ErrorMessage = "Cena musí být zadána")]
        public double price { get; set; }

        [Display(Name = "Počet")]
        [Required(ErrorMessage = "Počet musí být zadán")]
        public int quantity { get; set; }

        [Display(Name = "Aktivní")]
        public int isActive { get; set; }

        public static  async Task<List<Product>> GetProducts()
        {
            return await ORM.Select<Product>(Database.GetInstance().connection, "SELECT * FROM Product", new object[0]);
        }

        public async static Task<Product> GetProductFromApi(int id)
        {
            string url = "https://localhost:44394/json";
            using (var client = new HttpClient())
            {
                HttpResponseMessage Res = await client.GetAsync(url);
                if (Res.StatusCode == HttpStatusCode.OK)
                {
                    var res = Res.Content.ReadAsStringAsync().Result;
                    List<Product> products = JsonSerializer.Deserialize<List<Product>>(res);
                    return (products.Where(x => x.id == id).First());
                }
                else
                {
                    return null;
                }
            };
        }
        public async static Task<List<Product>> GetProductsFromApi()
        {
            string url = "https://localhost:44394/json";

            using (var client = new HttpClient())
            {
                HttpResponseMessage Res = await client.GetAsync(url);
                if (Res.StatusCode == HttpStatusCode.OK)
                {
                    var res = Res.Content.ReadAsStringAsync().Result;
                    return (JsonSerializer.Deserialize<List<Product>>(res));
                }
                else
                {
                    return null;
                }
            };
        }
        public async static Task<bool> ProductExist(string name)
        {
            List<Product> p = await Product.GetProductsFromApi();
            p = p.Where(x => x.name == name).ToList();

            if(p.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
