using Microsoft.AspNetCore.Mvc;

namespace _1.RoadToSenior.Api.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
