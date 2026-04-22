using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VolleyballShopApp.Core.Contracts;
using VolleyballShopApp.Models.Product;

namespace VolleyballShopApp.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly IFavoriteService favoritesService;

        public FavoritesController(IFavoriteService favoritesService)
        {
            this.favoritesService = favoritesService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            List<ProductIndexVM> products = favoritesService.GetFavorites(userId)
                .Select(f => new ProductIndexVM
                {
                    Id = f.ProductId,
                    ProductName = f.Product.ProductName,
                    BrandName = f.Product.Brand.BrandName,
                    CategoryName = f.Product.Category.CategoryName,
                    Picture = f.Product.Picture,
                    Description = f.Product.Description,
                    Quantity = f.Product.Quantity,
                    Price = f.Product.Price,
                    Discount = f.Product.Discount
                })
                .ToList();

            return View(products);
        }

        [HttpPost]
        public IActionResult Add(int productId)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            favoritesService.AddToFavorites(userId, productId);

            return RedirectToAction("Index", "Product");
        }

        [HttpPost]
        public IActionResult Remove(int productId)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            favoritesService.RemoveFromFavorites(userId, productId);

            return RedirectToAction(nameof(Index));
        }
    }
}