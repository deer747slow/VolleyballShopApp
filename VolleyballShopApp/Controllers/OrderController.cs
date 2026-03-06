using Microsoft.AspNetCore.Mvc;

namespace VolleyballShopApp.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
