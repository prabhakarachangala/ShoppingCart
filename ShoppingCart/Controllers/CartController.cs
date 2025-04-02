using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Data;
using ShoppingCart.Models;

namespace ShoppingCart.Controllers
{
    public class CartController : Controller
    {
        //public IActionResult Index()
        //{
        //    string userRole = HttpContext.Session.GetString("UserRole");

        //    ViewBag.UserRole = userRole; // Pass role to view
        //    return View();
        //}

        //public IActionResult CartContent()
        //{
        //    return PartialView("_Cart"); // Return partial view for cart content
        //}

        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }

            var cartItems = _context.Carts
                .Where(c => c.UserId == userId)
                .Select(c => new
                {
                    c.Id,
                    c.ProductId,
                    ProductName = c.Product.Name,
                    c.Quantity,
                    c.Product.Price
                })
                .ToList();

            return View(cartItems);
        }

        // Action to display all products in the cart (for the buyer)
        public IActionResult Cart()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        // Action to add product to the buyer's cart (session or cart table in DB)
        [HttpGet]
        public IActionResult AddToCart(int id) // Product ID
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var existingCartItem = _context.Carts
                .FirstOrDefault(c => c.UserId == userId && c.ProductId == id);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += 1;
            }
            else
            {
                var cartItem = new Cart
                {
                    UserId = userId.Value,
                    ProductId = id,
                    Quantity = 1
                };
                _context.Carts.Add(cartItem);
            }

            _context.SaveChanges();

            // Get product details for confirmation page
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            return View(product); // Show confirmation page
        }


        public IActionResult Checkout()
        {
            var cart = "";
            return View(cart);
        }
    }
}
