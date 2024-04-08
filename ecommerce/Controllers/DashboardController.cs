using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
