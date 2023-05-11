using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using projekt.Models;
using projekt.Models.Forms;
using projekt.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace projekt.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public async void Init()
        {
            await Database.GetInstance().Connect();
        }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            Init();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.TryGetValue("id", out byte[] userId) != false)
            {
                return RedirectToAction("Homepage", "Home");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User user)
        {
            if (HttpContext.Session.TryGetValue("id", out byte[] userid) != false)
            {
                return RedirectToAction("Homepage", "Home");
            }
            else if (ModelState.IsValid)
            {
                var user1 = (await ORM.Select<User>(Database.GetInstance().connection, "SELECT * FROM User WHERE email = @0 AND password = @1", user.email, user.password))?[0];
               
                if (user1 != null)
                {
                    HttpContext.Session.Set("id", BitConverter.GetBytes(user1.id));
                    return RedirectToAction("Homepage");
                }
                else
                {
                    ViewBag.Message = "Nepodařilo se přihlásit";
                }
            }

            return View(user);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Home");
        }

        public IActionResult Register()
        {
            if (HttpContext.Session.TryGetValue("id", out byte[] userid) != false)
            {
                return RedirectToAction("Homepage", "Home");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserForm form)
        {
            if (HttpContext.Session.TryGetValue("id", out byte[] userid) != false)
            {
                return RedirectToAction("Homepage", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    User u = new User()
                    {
                        fName = form.fName,
                        lName = form.lName,
                        email = form.email,
                        password = form.password,
                        phone = form.phone,
                        dateOfCreation = DateTime.Now.ToString(),
                        type = 3
                    };

                    if (await u.RegisterCheck())
                    {
                        Address address = new Address()
                        {
                            street = form.street,
                            city = form.city,
                            postalCode = form.postalCode,
                            country = form.country,
                        };

                        await ORM.Insert(Database.GetInstance().connection, address);

                        u.addressId = address.id;
                        await ORM.Insert(Database.GetInstance().connection, u);

                        HttpContext.Session.Set("id", BitConverter.GetBytes(u.id));
                        return RedirectToAction("Homepage");
                    }
                }
            }

            ViewData["Message"] = "Špatně zadané údaje";
            return View(form);
        }

        public async Task<IActionResult> HomePage()
        {
            if (HttpContext.Session.TryGetValue("id", out byte[] userid) != false)
            {
                HttpContext.Response.Headers.Add("Content-Type", "text/html; charset=utf-8");
                ViewData["userid"] = BitConverter.ToInt32(userid);
                ViewData["userType"] = (await ORM.Select<User>(Database.GetInstance().connection, "SELECT * FROM User WHERE id = @0", BitConverter.ToInt32(userid)))[0].type;

                List<SelectListItem> options = new List<SelectListItem>()
                {
                        new SelectListItem("Vzestupně", "0"),
                        new SelectListItem("Sestupně", "1"),
                };
                ViewBag.Options = options;

                List<Product> products = await Product.GetProductsFromApi();
                products = products.Where(x => x.isActive == 1).ToList();

                ViewBag.Products = products;

                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> HomePage(Sort form)
        {
            if (HttpContext.Session.TryGetValue("id", out byte[] userid) != false)
            {
                HttpContext.Response.Headers.Add("Content-Type", "text/html; charset=utf-8");
                ViewData["userid"] = BitConverter.ToInt32(userid);
                ViewData["userType"] = (await ORM.Select<User>(Database.GetInstance().connection, "SELECT * FROM User WHERE id = @0", BitConverter.ToInt32(userid)))[0].type;

                int x = form.orderBy;

                List<SelectListItem> options = new List<SelectListItem>()
                {
                        new SelectListItem("Vzestupně", "0", x == 0 ? true : false),
                        new SelectListItem("Sestupně", "1",  x == 1 ? true : false),
                };
                ViewBag.Options = options;

                List<Product> products = await Product.GetProductsFromApi();

                if (x == 0)
                {
                    if (form.sortByProperty != null)
                    {
                        if (form.sortByProperty == "price")
                        {
                            products = products.OrderBy(x => x.price).ToList();
                        }
                        else
                        {
                            products = products.OrderBy(x => x.quantity).ToList();
                        }
                    }
                }
                else
                {
                    if (form.sortByProperty != null)
                    {
                        if (form.sortByProperty == "price")
                        {
                            products = products.OrderByDescending(x => x.price).ToList();
                        }
                        else
                        {
                            products = products.OrderByDescending(x => x.quantity).ToList();
                        }
                    }
                }

                if (!String.IsNullOrEmpty(form.searchText))
                {
                    products = products.Where(x => x.name.ToLower().Contains(form.searchText.ToLower())).ToList();
                }

                ViewBag.Products = products;

                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public async Task<IActionResult> Profil()
        {
            if (HttpContext.Session.TryGetValue("id", out byte[] userid) != false)
            {
                ViewData["userid"] = BitConverter.ToInt32(userid);
                User u = (await ORM.Select<User>(Database.GetInstance().connection, "SELECT * FROM User WHERE id = @0", BitConverter.ToInt32(userid)))[0];
              
                List<Orders> orders = await ORM.Select<Orders>(Database.GetInstance().connection, "SELECT * FROM Orders WHERE userId = @0", BitConverter.ToInt32(userid));

                ViewBag.Orders = orders;

                ViewData["userType"] = u.type;
                ViewData["user"] = u;
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public async Task<IActionResult> Users(string SearchText, string SelectOption, int SelectOption2)
        {
            if (HttpContext.Session.TryGetValue("id", out byte[] userid) != false)
            {

                int type = (await ORM.Select<User>(Database.GetInstance().connection, "SELECT * FROM User WHERE id = @0", BitConverter.ToInt32(userid)))[0].type;

                ViewData["userType"] = type;
                ViewData["userid"] = BitConverter.ToInt32(userid);

                if (type == 2 || type == 1)
                {
                    List<User> users = await ORM.Select<User>(Database.GetInstance().connection, "SELECT * FROM User", new object[0]);

                    if (!String.IsNullOrEmpty(SearchText))
                    {
                        switch (SelectOption)
                        {
                            case "email":
                                {
                                    users = users.Where(x => x.email.Contains(SearchText)).ToList();
                                }
                                break;
                            case "name":
                                {
                                    users = users.Where(x => (x.fName.ToLower() + " " + x.lName.ToLower()).Contains(SearchText.ToLower())).ToList();
                                }
                                break;
                        }
                    }

                    if (SelectOption2 != 0)
                    {
                        users = users.Where(x => x.type.Equals(SelectOption2)).ToList();
                    }

                    ViewBag.Users = users;
                    return View();
                }
                else
                {
                    return RedirectToAction("Homepage", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public async Task<IActionResult> UserDetail(int id)
        {
            if (HttpContext.Session.TryGetValue("id", out byte[] userid) != false)
            {

                int type = (await ORM.Select<User>(Database.GetInstance().connection, "SELECT * FROM User WHERE id = @0", BitConverter.ToInt32(userid)))[0].type;
                ViewData["userType"] = type;
                ViewData["userid"] = BitConverter.ToInt32(userid);

                if (type == 2 || type == 1)
                {
                    User u = (await ORM.Select<User>(Database.GetInstance().connection, "SELECT * FROM User WHERE id = @0", id))[0];
                    Address a = (await ORM.Select<Address>(Database.GetInstance().connection, "SELECT * FROM Address WHERE id = @0", u.addressId))[0];

                    List<SelectListItem> types = new List<SelectListItem>()
                    {
                        new SelectListItem("Administrátor", "1", u.type == 1 ? true : false),
                        new SelectListItem("Pracovník prodejny", "2", u.type == 2 ? true : false),
                        new SelectListItem("Zákazník", "3", u.type == 3 ? true : false),
                    };

                    ViewBag.Options = types;

                    EditUserForm model = new EditUserForm()
                    {
                        fName = u.fName,
                        lName = u.lName,
                        phone = u.phone,
                        email = u.email,
                        type = u.type,
                        street = a.street,
                        city = a.city,
                        postalCode = a.postalCode,
                        country = a.country,
                    };

                    return View(model);
                }
                else
                {
                    return RedirectToAction("Homepage", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UserDetail(int id, EditUserForm form)
        {
            User u = (await ORM.Select<User>(Database.GetInstance().connection, "SELECT * FROM User WHERE id = @0", id))[0];
            Address a = (await ORM.Select<Address>(Database.GetInstance().connection, "SELECT * FROM Address WHERE id = @0", u.addressId))[0];

            List<SelectListItem> types = new List<SelectListItem>()
            {
                new SelectListItem("Administrátor", "1", u.type == 1 ? true : false),
                new SelectListItem("Pracovník prodejny", "2", u.type == 2 ? true : false),
                new SelectListItem("Zákazník", "3", u.type == 3 ? true : false),
            };

            if (HttpContext.Session.TryGetValue("id", out byte[] userid) != false)
            {
                if (ModelState.IsValid)
                {

                    u.fName = form.fName;
                    u.lName = form.lName;
                    u.phone = form.phone;
                    u.email = form.email;
                    u.type = form.type;

                    a.street = form.street;
                    a.country = form.country;
                    a.city = form.city;
                    a.postalCode = form.postalCode;

                    await ORM.Update(Database.GetInstance().connection, a);
                    await ORM.Update(Database.GetInstance().connection, u);

                    return RedirectToAction("Users", "Home");
                }
                else
                {
                    ViewBag.Options = types;
                    ViewData["Message"] = "Nezdařilo se uložení";
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public async Task<IActionResult> Products()
        {
            if (HttpContext.Session.TryGetValue("id", out byte[] userid) != false)
            {

                List<Product> products1 = await Product.GetProductsFromApi();

                int type = (await ORM.Select<User>(Database.GetInstance().connection, "SELECT * FROM User WHERE id = @0", BitConverter.ToInt32(userid)))[0].type;

                ViewData["userType"] = type;
                ViewData["userid"] = BitConverter.ToInt32(userid);

                List<SelectListItem> options = new List<SelectListItem>()
                {
                        new SelectListItem("Vzestupně", "0"),
                        new SelectListItem("Sestupně", "1"),
                };
                ViewBag.Options = options;

                if (type == 2 || type == 1)
                {

                    ViewBag.Products = products1;
                    return View();
                }
                else
                {
                    return RedirectToAction("Homepage", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Products(Sort form)
        {
            if (HttpContext.Session.TryGetValue("id", out byte[] userid) != false)
            {

                int type = (await ORM.Select<User>(Database.GetInstance().connection, "SELECT * FROM User WHERE id = @0", BitConverter.ToInt32(userid)))[0].type;

                ViewData["userType"] = type;
                ViewData["userid"] = BitConverter.ToInt32(userid);

                int x = form.orderBy;

                List<SelectListItem> options = new List<SelectListItem>()
                {
                        new SelectListItem("Vzestupně", "0", x == 0 ? true : false),
                        new SelectListItem("Sestupně", "1",  x == 1 ? true : false),
                };
                ViewBag.Options = options;

                List<Product> products = await Product.GetProductsFromApi();

                if (x == 0)
                {
                    if (form.sortByProperty != null)
                    {
                        if (form.sortByProperty == "price")
                        {
                            products = products.OrderBy(x => x.price).ToList();
                        }
                        else if (form.sortByProperty == "quantity")
                        {
                            products = products.OrderBy(x => x.quantity).ToList();
                        }
                        else
                        {
                            products = products.Where(x => x.isActive == 1).ToList();
                        }
                    }
                }
                else
                {
                    if (form.sortByProperty != null)
                    {
                        if (form.sortByProperty == "price")
                        {
                            products = products.OrderByDescending(x => x.price).ToList();
                        }
                        else if (form.sortByProperty == "quantity")
                        {
                            products = products.OrderByDescending(x => x.quantity).ToList();
                        }
                        else
                        {
                            products = products.Where(x => x.isActive == 1).ToList();
                        }
                    }
                }

                if (!String.IsNullOrEmpty(form.searchText))
                {
                    products = products.Where(x => x.name.ToLower().Contains(form.searchText.ToLower())).ToList();
                }

                if (type == 2 || type == 1)
                {

                    ViewBag.Products = products;
                    return View();
                }
                else
                {
                    return RedirectToAction("Homepage", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public async Task<IActionResult> DeleteProduct(int id)
        {

            Product product = await Product.GetProductFromApi(id);

            if (product == null)
            {
                return RedirectToAction("Products");
            }

            product.isActive = 0;
            await ORM.Update(Database.GetInstance().connection, product);
            return RedirectToAction("Products");

        }

        public async Task<IActionResult> ProductDetail(int id)
        {
            if (HttpContext.Session.TryGetValue("id", out byte[] userid) != false)
            {

                int type = (await ORM.Select<User>(Database.GetInstance().connection, "SELECT * FROM User WHERE id = @0", BitConverter.ToInt32(userid)))[0].type;
                ViewData["userType"] = type;
                ViewData["userid"] = BitConverter.ToInt32(userid);

                Product product = await Product.GetProductFromApi(id);
                if (product == null)
                {
                    return RedirectToAction("Homepage");
                }

                List<SelectListItem> options = new List<SelectListItem>()
                {
                    new SelectListItem("Ano", "1", product.isActive == 1 ? true : false),
                    new SelectListItem("Ne", "0", product.isActive == 0 ? true : false),
                };
                ViewBag.Options = options;


                if (type == 2 || type == 1)
                {
                    return View(product);
                }
                else
                {
                    return RedirectToAction("Homepage", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ProductDetail(Product form)
        {
            List<SelectListItem> options = new List<SelectListItem>()
                {
                    new SelectListItem("Ano", "1", form.isActive == 1 ? true : false),
                    new SelectListItem("Ne", "0", form.isActive == 0 ? true : false),
                };

            if (HttpContext.Session.TryGetValue("id", out byte[] userid) != false)
            {
                if (ModelState.IsValid)
                {
                    await ORM.Update(Database.GetInstance().connection, form);
                    return RedirectToAction("Products", "Home");
                }
                else
                {
                    ViewBag.Options = options;
                    ViewData["Message"] = "Nezdařilo se uložení";
                    return View(form);
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public async Task<IActionResult> CreateProduct()
        {
            if (HttpContext.Session.TryGetValue("id", out byte[] userid) != false)
            {
                int type = (await ORM.Select<User>(Database.GetInstance().connection, "SELECT * FROM User WHERE id = @0", BitConverter.ToInt32(userid)))[0].type;

                if (type == 2 || type == 1)
                {
                    List<SelectListItem> options = new List<SelectListItem>();
                    foreach (var cat in (await ORM.Select<Category>(Database.GetInstance().connection, "SELECT * FROM Category", new object[0])))
                    {
                        options.Add(new SelectListItem(cat.name, cat.id.ToString()));
                    }
                    ViewBag.Options = options;
                    return View();
                }
                else
                {
                    return RedirectToAction("Homepage");
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductForm form)
        {
            if (HttpContext.Session.TryGetValue("id", out byte[] userid) != false)
            {
                if (ModelState.IsValid)
                {
                    Product product = new Product()
                    {
                        name = form.product.name,
                        manufacturer = form.product.manufacturer,
                        description = form.product.description,
                        weight = form.product.weight,
                        price = form.product.price,
                        colour = form.product.colour,
                        quantity = form.product.quantity,
                        isActive = form.product.isActive,
                    };

                    if (await Product.ProductExist(product.name) == true)
                    {
                        ViewData["Message"] = "Produkt už existuje";
                        return View(form);
                    }

                    await ORM.Insert(Database.GetInstance().connection, product);

                    ProductCategory productCategory = new ProductCategory()
                    {
                        productId = product.id,
                        categoryId = form.catId
                    };

                    await ORM.Insert(Database.GetInstance().connection, productCategory);

                    return RedirectToAction("Products", "Home");
                }
                else
                {
                    ViewData["Message"] = "Nezdařilo se přidání";
                    List<SelectListItem> options = new List<SelectListItem>();
                    foreach (var cat in (await ORM.Select<Category>(Database.GetInstance().connection, "SELECT * FROM Category", new object[0])))
                    {
                        options.Add(new SelectListItem(cat.name, cat.id.ToString()));
                    }
                    ViewBag.Options = options;
                    return View(form);
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public async Task<IActionResult> CreateUser()
        {
            if (HttpContext.Session.TryGetValue("id", out byte[] userid) != false)
            {
                int type = (await ORM.Select<User>(Database.GetInstance().connection, "SELECT * FROM User WHERE id = @0", BitConverter.ToInt32(userid)))[0].type;

                if (type == 2 || type == 1)
                {
                    List<SelectListItem> types = new List<SelectListItem>()
                    {
                        new SelectListItem("Administrátor", "1"),
                        new SelectListItem("Pracovník prodejny", "2"),
                        new SelectListItem("Zákazník", "3"),
                    };
                    ViewBag.Options = types;
                    return View();
                }
                else
                {
                    return RedirectToAction("Homepage");
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserForm form)
        {
            if (HttpContext.Session.TryGetValue("id", out byte[] userid) != false)
            {
                if (ModelState.IsValid)
                {
                    User user = new User()
                    {
                        fName = form.user.fName,
                        lName = form.user.lName,
                        email = form.user.email,
                        phone = form.user.phone,
                        type = form.user.type,
                        password = form.user.password,
                        dateOfCreation = DateTime.Now.ToString(),
                    };

                    if (await user.RegisterCheck())
                    {
                        Address address = new Address()
                        {
                            street = form.address.street,
                            city = form.address.city,
                            postalCode = form.address.postalCode,
                            country = form.address.country,
                        };

                        await ORM.Insert(Database.GetInstance().connection, address);
                        user.addressId = address.id;
                        await ORM.Insert(Database.GetInstance().connection, user);
                        return RedirectToAction("Users", "Home");
                    }
                }
            }
            else
            {
                return RedirectToAction("Login");
            }

            ViewData["Message"] = "Nezdařilo se přidání";
            List<SelectListItem> types = new List<SelectListItem>()
                    {
                        new SelectListItem("Administrátor", "1"),
                        new SelectListItem("Pracovník prodejny", "2"),
                        new SelectListItem("Zákazník", "3"),
                    };
            ViewBag.Options = types;
            return View(form);
        }

        public async Task<IActionResult> Orders()
        {
            if (HttpContext.Session.TryGetValue("id", out byte[] userid) != false)
            {
                int type = (await ORM.Select<User>(Database.GetInstance().connection, "SELECT * FROM User WHERE id = @0", BitConverter.ToInt32(userid)))[0].type;

                ViewData["userType"] = type;
                ViewData["userid"] = BitConverter.ToInt32(userid);

                if (type == 2 || type == 1)
                {
                    List<Orders> orders = await ORM.Select<Orders>(Database.GetInstance().connection, "SELECT * FROM Orders", new object[0]);
                    ViewBag.Orders = orders;
                    return View();
                }
                else
                {
                    return RedirectToAction("Homepage", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public async Task<IActionResult> DeleteOrders(int id)
        {

            Orders order = (await ORM.Select<Orders>(Database.GetInstance().connection, "SELECT * FROM Orders WHERe id = @0", id))[0];

            if (order == null)
            {
                return RedirectToAction("Orders");
            }

            order.state = "Cancelled";
            await ORM.Update(Database.GetInstance().connection, order);
            return RedirectToAction("Orders");

        }

        public async Task<IActionResult> Detail(int id)
        {
            if (HttpContext.Session.TryGetValue("id", out byte[] userid) != false)
            {
                int type = (await ORM.Select<User>(Database.GetInstance().connection, "SELECT * FROM User WHERE id = @0", BitConverter.ToInt32(userid)))[0].type;
                ViewData["userType"] = type;
                ViewData["userid"] = BitConverter.ToInt32(userid);

                Product product = await Product.GetProductFromApi(id);


                ViewBag.Product = product;

                List<ProductCategory> categories = await ORM.Select<ProductCategory>(Database.GetInstance().connection, "SELECT * FROM ProductCategory");
                categories = categories.Where(x => x.productId == product.id).ToList();

                Category category = null;
                if (categories.Count != 0)
                {
                    category = (await ORM.Select<Category>(Database.GetInstance().connection, "SELECT * FROM Category WHERE id = @0", categories[0].categoryId))[0];
                    ViewBag.Category = category.name;

                    return View(new CartItemForm()
                    {
                        count = 1,
                        productId = product.id,
                        productName = product.name
                    });
                }

                ViewBag.Category = "";

                return View(new CartItemForm()
                {
                    count = 1,
                    productId = product.id,
                    productName = product.name
                });
            }
            else
            {
                ViewBag.Message = "Musíte být přihlášen";
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Detail(int id, CartItemForm form)
        {
            if (HttpContext.Session.TryGetValue("id", out byte[] userid) != false)
            {
                if (form.count <= 0)
                {
                    ModelState.AddModelError("count", "Minimální množství kusů je 1");
                }

                if (ModelState.IsValid)
                {
                    string tmp = this.HttpContext.Session.GetString("shoppingCart");
                    if (tmp == null)
                    {
                        this.HttpContext.Session.SetString("shoppingCart", JsonSerializer.Serialize(new List<CartItemForm>() {
                        form
                    })
                      );

                    }
                    else
                    {
                        List<CartItemForm> cartItemForms = new List<CartItemForm>();

                        foreach (var v in JsonSerializer.Deserialize<List<CartItemForm>>(tmp))
                        {
                            if (v.productId != form.productId)
                            {
                                cartItemForms.Add(v);
                            }
                            else
                            {
                                form.count += v.count;
                            }
                        }

                        cartItemForms.Add(form);
                        this.HttpContext.Session.SetString("shoppingCart", JsonSerializer.Serialize(cartItemForms)
                     );
                    }


                    return RedirectToAction("ShoppingCart", "Home");
                }


                Product product = await Product.GetProductFromApi(id);
                if (product == null)
                {
                    return RedirectToAction("Homepage", "Home");
                }

                ViewBag.Product = product;
                return View(form);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public async Task<IActionResult> ShoppingCart()
        {
            if (HttpContext.Session.TryGetValue("id", out byte[] userid) != false)
            {
                string json = this.HttpContext.Session.GetString("shoppingCart");

                List<SelectListItem> users = new List<SelectListItem>();
                foreach (var u in (await ORM.Select<User>(Database.GetInstance().connection, "SELECT * FROM User", new object[0])))
                {
                    users.Add(new SelectListItem(u.fName + " " + u.lName, u.id.ToString()));
                }

                ViewBag.Users = users;

                if (json == null)
                {
                    ViewBag.kosik = null;                
                    return View();
                }

                List<CartItemForm> kosik = JsonSerializer.Deserialize<List<CartItemForm>>(json);

                ViewBag.kosik = kosik;
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ShoppingCart(OrderForm form)
        {
            string json = this.HttpContext.Session.GetString("shoppingCart");
            List<CartItemForm> kosik = JsonSerializer.Deserialize<List<CartItemForm>>(json);

            if (HttpContext.Session.TryGetValue("id", out byte[] userid) != false)
            {
                if (ModelState.IsValid)
                {
                    Orders order = new Orders()
                    {
                        dateOfCreation = DateTime.Now.ToString(),
                        state = "Accepted",
                        userId = form.id,
                        totalPrice = 0
                    };


                    List<OrderItem> orderItems = new List<OrderItem>();
                    double totalPrice = 0;

                    foreach (var item in kosik)
                    {
                        orderItems.Add(new OrderItem()
                        {
                            name = item.productName,
                            productId = item.productId,
                            quantity = item.count
                        });
                        Product product = await Product.GetProductFromApi(item.productId);

                        if (product.quantity < item.count)
                        {
                            return RedirectToAction("Homepage");
                        }
                        product.quantity -= item.count;
                        await ORM.Update(Database.GetInstance().connection, product);

                        totalPrice += item.count * product.price;

                    }

                    order.totalPrice = totalPrice;

                    await ORM.Insert(Database.GetInstance().connection, order);
                    foreach (OrderItem item in orderItems)
                    {
                        item.orderId = order.id;
                        await ORM.Insert(Database.GetInstance().connection, item);
                    }

                    this.HttpContext.Session.Remove("shoppingCart");
                    return RedirectToAction("Done");
                }

                SelectList list = new SelectList(await ORM.Select<User>(Database.GetInstance().connection, "SELECT * FROM User", new object[0]));

                ViewBag.Users = list;

                ViewBag.kosik = kosik;
                return View(form);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public IActionResult Done()
        {
            if (HttpContext.Session.TryGetValue("id", out byte[] userid) != false)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public async Task<IActionResult> OrderDetail(int id)
        {
            if (HttpContext.Session.TryGetValue("id", out byte[] userid) != false)
            {
                int type = (await ORM.Select<User>(Database.GetInstance().connection, "SELECT * FROM User WHERE id = @0", BitConverter.ToInt32(userid)))[0].type;

                Orders order = (await ORM.Select<Orders>(Database.GetInstance().connection, "SELECT * FROM Orders WHERE id = @0", id))[0];
                User user = (await ORM.Select<User>(Database.GetInstance().connection, "SELECT * FROM User WHERE id = @0", order.userId))[0];
                List<OrderItem> items = await ORM.Select<OrderItem>(Database.GetInstance().connection, "SELECT * FROM OrderItem WHERE orderId = @0", order.id);

                EditOrderForm editOrderForm = new EditOrderForm()
                {
                    state = order.state,
                    fName = user.fName,
                    lName = user.lName,
                    totalPrice = order.totalPrice
                };

                editOrderForm.items = new List<CartItemForm>();

                foreach (var item in items)
                {
                    CartItemForm cartItemForm = new CartItemForm()
                    {
                        productId = item.productId,
                        count = item.quantity,
                        productName = item.name
                    };
                    editOrderForm.items.Add(cartItemForm);
                }

                List<SelectListItem> states = new List<SelectListItem>()
                {
                    new SelectListItem("Accepted", "Accepted", order.state == "Accepted" ? true : false),
                    new SelectListItem("Cancelled", "Cancelled", order.state == "Cancelled" ? true : false),
                    new SelectListItem("Paid", "Paid", order.state == "Paid" ? true : false),
                    new SelectListItem("Finished", "Finished", order.state == "Finished" ? true : false),
                };
                ViewBag.States = states;


                if (type == 1 || type == 2)
                {
                    return View(editOrderForm);
                }
                else
                {
                    return RedirectToAction("Homepage");
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        [HttpPost]
        public async Task<IActionResult> OrderDetail(int id, EditOrderForm form)
        {
            if (HttpContext.Session.TryGetValue("id", out byte[] userid) != false)
            {
                if (ModelState.IsValid)
                {
                    Orders order = (await ORM.Select<Orders>(Database.GetInstance().connection, "SELECT * FROM Orders WHERE id = @0", id))[0];
                    order.state = form.state;
                    order.dateOfCreation = DateTime.Now.ToString();

                    await ORM.Update(Database.GetInstance().connection, order);

                    return RedirectToAction("Orders");
                }
                else
                {
                    List<SelectListItem> states = new List<SelectListItem>()
                    {
                        new SelectListItem("Accepted", "Accepted", form.state == "Accepted" ? true : false),
                        new SelectListItem("Cancelled", "Cancelled", form.state == "Cancelled" ? true : false),
                        new SelectListItem("Paid", "Paid", form.state == "Paid" ? true : false),
                        new SelectListItem("Finished", "Finished", form.state == "Finished" ? true : false),
                    };
                    ViewBag.States = states;
                    return View(form);
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public IActionResult ClearShoppingCart()
        {
            this.HttpContext.Session.Remove("shoppingCart");
            return RedirectToAction("ShoppingCart");
        }
    }
}
