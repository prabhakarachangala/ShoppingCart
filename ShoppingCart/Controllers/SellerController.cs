using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Models;

namespace ShoppingCart.Controllers
{
    public class SellerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SellerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Get all products from the database
            var products = _context.Products.ToList();
            return View(products);
        }

        [HttpPost]
        public IActionResult Register(SellerProfile sellerProfile)
        {
            if (ModelState.IsValid)
            {
                // Save the seller profile to the database
                _context.SellerProfiles.Add(sellerProfile);
                _context.SaveChanges();
                return RedirectToAction("Dashboard", "Seller");
            }
            return View(sellerProfile);
        }

        public IActionResult AddProduct()
        {
            return View();  // This will return the AddProduct view
        }

        [HttpPost]
        public IActionResult UploadProduct(Product product, IFormFile image)
        {
            var sellerId = HttpContext.Session.GetInt32("SellerId");

            if (sellerId == null)
            {
                // Handle if seller is not logged in
                return RedirectToAction("Login", "User");
            }

            // Assign the logged-in seller's ID to the product
            product.SellerId = sellerId.Value;

            if (image != null && image.Length > 0)
            {
                // Define the directory path
                var imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                // Ensure the directory exists (create if it doesn't)
                if (!Directory.Exists(imageDirectory))
                {
                    Directory.CreateDirectory(imageDirectory);  // This will create the images directory if it doesn't exist
                }

                // Generate a unique file name to avoid collisions
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var filePath = Path.Combine(imageDirectory, fileName);

                // Save the image to the directory
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }

                // Save the image path to the product
                product.ImageUrl = "/images/" + fileName;  // Save the image path
            }

            // Add the product to the database
            _context.Products.Add(product);
            _context.SaveChanges();

            // Redirect to the dashboard or another view
            return RedirectToAction("Index");
        }
    }

}
