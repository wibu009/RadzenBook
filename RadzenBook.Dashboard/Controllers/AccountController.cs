using Microsoft.AspNetCore.Mvc;

namespace RadzenBook.Dashboard.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

    }
}
