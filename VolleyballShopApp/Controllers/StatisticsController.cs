using Microsoft.AspNetCore.Mvc;

namespace VolleyballShopApp.Controllers
{
    public class StatisticsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
