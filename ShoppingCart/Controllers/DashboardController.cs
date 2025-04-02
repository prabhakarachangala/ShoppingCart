using Microsoft.AspNetCore.Mvc;

namespace ShoppingCart.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult AdminDashboard()
        {
            return View();
        }

        public IActionResult SellerDashboard()
        {
            return View();
        }

        public IActionResult BuyerDashboard()
        {
            return View();
        }
    }
}
