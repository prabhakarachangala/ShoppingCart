using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Data;
using ShoppingCart.Models;

namespace ShoppingCart.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Users
        public IActionResult Index()
        {
            return View(_context.Users.ToList());
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


        // LOGIN - Show Login Page
        public IActionResult Login()
        {
            HttpContext.Session.SetString("UserRole", "Buyer");
            return View();
        }

        //// LOGIN - Authenticate User
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Login(LoginViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = _context.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

        //        if (user != null)
        //        {
        //            // Store User Info in Session
        //            HttpContext.Session.SetString("Username", user.Username);
        //            HttpContext.Session.SetString("Role", user.Role);

        //            // Redirect Based on Role
        //            if (user.Role == "Admin")
        //                return RedirectToAction("AdminDashboard", "Dashboard");
        //            else if (user.Role == "Seller")
        //                return RedirectToAction("SellerDashboard", "Dashboard");
        //            else
        //                return RedirectToAction("BuyerDashboard", "Dashboard");
        //        }

        //        ViewBag.ErrorMessage = "Invalid Username or Password";
        //    }
        //    return View(model);
        //}

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            var user = ValidateUser(model.Username, model.Password);
            if (user != null)
            {
                // Store user info in session
                HttpContext.Session.SetString("UserRole", user.Role);
                HttpContext.Session.SetString("Username", user.Username);

                if (user.Role == "Seller")
                {
                    HttpContext.Session.SetInt32("SellerId", user.Id);
                }
                else
                {
                    HttpContext.Session.SetInt32("UserId", user.Id);
                }

                    // Set the session data into ViewData for layout access
                    ViewData["UserRole"] = user.Role;
                ViewData["Username"] = user.Username;

                // Redirect based on the role
                if (user.Role == "Seller")
                {
                    return RedirectToAction("Index", "Seller"); // Redirect to Seller's dashboard
                }
                else if (user.Role == "Admin")
                {
                    return RedirectToAction("Index", "Admin"); // Redirect to Admin's dashboard
                }
                else
                {
                    return RedirectToAction("Cart", "Cart"); // Default redirect for other roles (e.g., Buyer)
                }
            }

            ViewBag.Message = "Invalid login credentials.";
            return View();
        }

        // LOGOUT - Clear Session
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult Dashboard()
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            // Do something with the user role (e.g., show different content based on the role)
            return View();
        }

        private User ValidateUser(string username, string password)
        {
            // Use ToLower() to compare case-insensitively
            var user = _context.Users
                .FirstOrDefault(u => u.Username.ToLower() == username.ToLower() && u.Password == password);


            return user;
        }
    }
}
