using Microsoft.AspNetCore.Mvc;

namespace RadzenBook.Dashboard.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public ActionResult View()
        {
            return View();
        }


        public ActionResult Partial_SanPham()
        {
            return PartialView();
        }
    }
}
