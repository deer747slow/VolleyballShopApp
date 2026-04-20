using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VolleyballShopApp.Infrastructure.Data;
using VolleyballShopApp.Infrastructure.Data.Entities;

namespace VolleyballShopApp.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FavoritesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Add(int productId)
        {
            var user = await _userManager.GetUserAsync(User);

            var exists = _context.Favorites
                .Any(f => f.UserId == user.Id && f.ProductId == productId);

            if (!exists)
            {
                var favorite = new Favorite
                {
                    UserId = user.Id,
                    ProductId = productId
                };

                _context.Favorites.Add(favorite);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Products");
        }
    }
}
