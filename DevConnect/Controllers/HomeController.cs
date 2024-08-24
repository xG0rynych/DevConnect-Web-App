using Microsoft.AspNetCore.Mvc;

namespace DevConnect.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult AboutMe()
        {
            return View();
        }

        public IActionResult ErrorView()
        {
            return View();
        }
    }
}
