using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Data;
using ShoppingCart.Models;

namespace ShoppingCart.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Action to view all users and sellers
        public IActionResult Index()
        {
            // Get all users (including sellers and admins) and their activation status
            var users = _context.Users.ToList();
            return View(users);
        }

        // Action to view all users
        public IActionResult Users()
        {
            var users = _context.Users.ToList();  // Get all users
            return View(users);  // Return the view to display the users
        }

        // Action to activate/deactivate a user
        [HttpPost]
        public IActionResult ToggleUserActivation(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                user.IsActive = !user.IsActive;  // Toggle the IsActive flag
                _context.SaveChanges();  // Save changes to the database
            }
            return RedirectToAction("Users");  // Redirect to users page
        }

        // Action to view sellers (can be separate or integrated with users as shown)
        public IActionResult Sellers()
        {
            var sellers = _context.Users.Where(u => u.Role == "Seller").ToList();
            return View(sellers);  // Return the view to display sellers
        }

        // Action to activate/deactivate a seller
        [HttpPost]
        public IActionResult ToggleSellerActivation(int id)
        {
            var seller = _context.Users.Find(id);
            if (seller != null && seller.Role == "Seller")
            {
                seller.IsActive = !seller.IsActive;  // Toggle the IsActive flag for sellers
                _context.SaveChanges();  // Save changes to the database
            }
            return RedirectToAction("Sellers");  // Redirect to sellers page
        }


        // GET: Create User
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create User
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Edit User
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Edit User
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Delete User
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Delete User
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
