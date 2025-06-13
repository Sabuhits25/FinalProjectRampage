using Microsoft.AspNetCore.Mvc;

namespace Rampage.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
