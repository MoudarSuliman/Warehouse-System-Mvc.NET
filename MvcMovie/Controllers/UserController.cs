using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

public class UserController : Controller
{
    private string _usersFilePath = @"user.json";
    private int _nextUserId = 1;

    private List<User> GetUsers()
    {
        if (System.IO.File.Exists(_usersFilePath))
        {
            string json = System.IO.File.ReadAllText(_usersFilePath);
            return JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
        }

        return new List<User>();
    }

    private void SaveUsers(List<User> users)
    {
        string json = JsonConvert.SerializeObject(users);
        System.IO.File.WriteAllText(_usersFilePath, json);
    }

    public ActionResult Login()
    {
        return View();
    }


    [HttpPost]
    public ActionResult Login(User user)
    {
        var users = GetUsers();
        var existingUser = users.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);

        if (existingUser == null)
        {
            ModelState.AddModelError("Login", "Invalid login credentials.");
            return View();
        }

        TempData["LoggedInUsername"] = existingUser.Username;

        return RedirectToAction("Index", "Product");
    }


    public ActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Register(User user)
    {
        if (!ModelState.IsValid)
        {
            return View(user);
        }

        var users = GetUsers();

        if (users.Any(u => u.Username == user.Username))
        {
            ModelState.AddModelError("Username", "Username already exists.");
            return View(user);
        }

        // Generate a unique ID for the user
        int nextUserId = users.Any() ? users.Max(u => u.Id) + 1 : 1;
        user.Id = nextUserId;

        // Add the new user to the list
        users.Add(user);

        // Save the updated user list to the JSON file
        SaveUsers(users);


        TempData["UserId"] = user.Id;

        return RedirectToAction("Login");
    }

    public ActionResult Delete()
    {
        var loggedInUsername = (string)TempData["LoggedInUsername"];
        var users = GetUsers();
        var userToDelete = users.FirstOrDefault(u => u.Username == loggedInUsername);
        if (userToDelete != null)
        {
            users.Remove(userToDelete);
        }

        // Save the updated user list to the JSON file
        SaveUsers(users);

        TempData.Clear();

        return RedirectToAction("Login");
    }

}
public class ProductController : Controller
{
    private string _productsFilePath = @"product.json";
    private int _nextProductId = 1;

    private List<Product> GetProducts()
    {
        if (System.IO.File.Exists(_productsFilePath))
        {
            string json = System.IO.File.ReadAllText(_productsFilePath);
            return JsonConvert.DeserializeObject<List<Product>>(json) ?? new List<Product>();
        }

        return new List<Product>();
    }


    private void SaveProducts(List<Product> products)
    {
        string json = JsonConvert.SerializeObject(products);
        System.IO.File.WriteAllText(_productsFilePath, json);
    }

    public ActionResult Index()
    {
        var products = GetProducts();

        float warehouseValue = products.Sum(p => p.Quantity * p.UnitPrice);
        float warehouseUnits = products.Sum(p => p.Quantity);

        var categorySummary = products
            .GroupBy(p => p.Category)
            .Select(g => new
            {
                Name = g.Key,
                Value = g.Sum(p => p.Quantity * p.UnitPrice),
                Units = g.Sum(p => p.Quantity)
            })
            .ToList();

        ViewBag.WarehouseValue = warehouseValue;
        ViewBag.WarehouseUnits = warehouseUnits;
        ViewBag.CategorySummary = categorySummary;

        return View(products);
    }


    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(Product product)
    {
        if (!ModelState.IsValid)
        {
            return View(product);
        }


        var products = GetProducts();

        // Check if the product code is already taken
        if (products.Any(p => p.Code == product.Code))
        {
            ModelState.AddModelError("Code", "The product code must be unique.");
            return View(product);
        }

        //Check the length of the Code added
        if (product.Code.Length > 8)
        {
            ModelState.AddModelError("Code", "The product code must be up to 8 char");
            return View(product);
        }

        // Check the length of the product name
        if (product.Name.Length > 16)
        {
            ModelState.AddModelError("Name", "The product name must be up to 16 characters.");
            return View(product);
        }


        // Validate the product quantity
        if (product.Quantity <= 0)
        {
            ModelState.AddModelError("Quantity", "The product quantity must be greater than zero.");
            return View(product);
        }

        // Validate the unit of measure
        string[] validUnitOfMeasure = { "kg", "pcs", "litre" };
        if (!validUnitOfMeasure.Contains(product.UnitOfMeasure))
        {
            ModelState.AddModelError("UnitOfMeasure", "Invalid unit of measure. Only usage of kg,pcs,litre is avaialable");
            return View(product);
        }

        // Generate a unique ID for the product
        int nextProductId = products.Any() ? products.Max(p => p.Id) + 1 : 1;
        product.Id = nextProductId;


        // Add the new product to the list
        products.Add(product);

        // Save the updated product list to the JSON file
        SaveProducts(products);

        return RedirectToAction("Index");
    }


    public ActionResult Edit(int id)
    {
        var products = GetProducts();
        var product = products.FirstOrDefault(p => p.Id == id);
        return View(product);
    }

    [HttpPost]
    public ActionResult Edit(Product product)
    {
        if (!ModelState.IsValid)
        {
            return View(product);
        }

        var products = GetProducts();
        var existingProduct = products.FirstOrDefault(p => p.Id == product.Id);
        if (existingProduct != null)
        {
            // Check if the product code is already taken
            if (products.Any(p => p.Id != existingProduct.Id && p.Code == product.Code))
            {
                ModelState.AddModelError("Code", "The product code must be unique.");
                return View(product);
            }

            // Check the length of the Code added
            if (product.Code.Length > 8)
            {
                ModelState.AddModelError("Code", "The product code must be up to 8 characters.");
                return View(product);
            }

            // Check the length of the product name
            if (product.Name.Length > 16)
            {
                ModelState.AddModelError("Name", "The product name must be up to 16 characters.");
                return View(product);
            }

            // Validate the product quantity
            if (product.Quantity <= 0)
            {
                ModelState.AddModelError("Quantity", "The product quantity must be greater than zero.");
                return View(product);
            }

            // Validate the unit of measure
            string[] validUnitOfMeasure = { "kg", "pcs", "litre" };
            if (!validUnitOfMeasure.Contains(product.UnitOfMeasure))
            {
                ModelState.AddModelError("UnitOfMeasure", "Invalid unit of measure. Only usage of kg, pcs, litre is available.");
                return View(product);
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Category = product.Category;
            existingProduct.Quantity = product.Quantity;
            existingProduct.UnitOfMeasure = product.UnitOfMeasure;
            existingProduct.UnitPrice = product.UnitPrice;

            // Save the updated product list to the JSON file
            SaveProducts(products);
        }

        return RedirectToAction("Index");
    }


    public ActionResult Delete(int id)
    {
        var products = GetProducts();
        var productToDelete = products.FirstOrDefault(p => p.Id == id);
        if (productToDelete != null)
        {
            products.Remove(productToDelete);

            // Save the updated product list to the JSON file
            SaveProducts(products);
        }

        return RedirectToAction("Index");
    }
}
