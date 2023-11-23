using Microsoft.AspNetCore.Mvc;

namespace RadzenBook.Dashboard.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
